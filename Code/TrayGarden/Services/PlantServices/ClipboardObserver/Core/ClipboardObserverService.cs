using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.Plants;
using TrayGarden.Reception.Services;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Services.PlantServices.ClipboardObserver.Core;

[UsedImplicitly]
public class ClipboardObserverService : PlantServiceBase<ClipboardObserverPlantBox>
{
  /// <summary>
  /// This field is used in 2 cases.
  /// If value is positive, it means that last CB value was greater than limit.
  /// If value is zero or negative, each value has special meaning:
  /// 0 - initial state, no updates before.
  /// -1 - last value was less than limit, lastKnownClipboardValue should be checked;
  /// -2 - last value was null.
  /// </summary>
  protected volatile int lastKnownClipboardLengthOrSpecial;

  protected volatile string lastKnownClipboardValue;

  protected volatile bool supressNextEvent;

  protected object timerLock = new object();

  public ClipboardObserverService()
    : base("Clipboard Observer", "ClipboardObserverService")
  {
    ServiceDescription = "Service monitors the clipboard and deliver change notifications to plants.";
    MaxAllowedTextLength = 100000;
    supressNextEvent = false;
    SelfProvider = new ClipboardProvider(this);
    Monitor = new ClipboardMonitor();
    Monitor.ClipboardValueChanged += OnClipboardValueChanged;
    ThreadQueue = new Queue<Action>();
    BufferTimeBeforeReact = 500;
    ClipboardPostponedReactTimer = new System.Threading.Timer(
      OnClipboardValueChangedHandler,
      null,
      Timeout.Infinite,
      Timeout.Infinite);
    //0 is used if this is initial state
    lastKnownClipboardLengthOrSpecial = 0;
    lastKnownClipboardValue = null;
  }

  //public int CheckIntervalMsec { get; set; }

  /// <summary>
  /// Specifies a time to wait after last clipboard change event. Sometimes we receive a lot of noise from Monitor.
  /// </summary>
  public int BufferTimeBeforeReact { get; set; }

  public int MaxAllowedTextLength { get; set; }

  protected System.Threading.Timer ClipboardPostponedReactTimer { get; set; }

  protected Thread ClipboardWorkingThread { get; set; }

  protected IGardenbed Gardenbed { get; set; }

  protected ClipboardMonitor Monitor { get; set; }

  protected IClipboardProvider SelfProvider { get; set; }

  protected Queue<Action> ThreadQueue { get; set; }

  public virtual string GetClipboardValue(bool disableSizeCheck)
  {
    string value = string.Empty;
    var awaiter = new ManualResetEventSlim(false);
    PostToClipboardThread(
      delegate
      {
        var clipboardValue = Clipboard.GetText();
        try
        {
          if (clipboardValue.IsNullOrEmpty())
          {
            return;
          }
          if (!disableSizeCheck && clipboardValue.Length > MaxAllowedTextLength)
          {
            return;
          }
          value = clipboardValue;
        }
        finally
        {
          awaiter.Set();
        }
      });
    awaiter.Wait();
    return value;
  }

  public override void InformDisplayStage()
  {
    base.InformDisplayStage();
    ClipboardWorkingThread.Start();
  }

  public override void InformInitializeStage()
  {
    base.InformInitializeStage();
    Gardenbed = HatcherGuide<IGardenbed>.Instance;
    InitializeClipboardWorkingThread();
  }

  public override void InitializePlant(IPlantEx plantEx)
  {
    base.InitializePlant(plantEx);
    InitializePlantWithLuggage(plantEx);
  }

  public virtual void SetClipboardValue(string newValue, bool silent)
  {
    PostToClipboardThread(
      delegate
      {
        supressNextEvent = silent;
        Clipboard.SetText(newValue);
      });
  }

  /// <summary>
  /// This is a worker thread for clipboard.
  /// Clipboard works should be performed only from STA thread, so I run a dedicated thread.
  /// </summary>
  protected virtual void CheckThreadLoop()
  {
    while (true)
    {
      try
      {
        Action callbackToHandle;
        lock (ThreadQueue)
        {
          if (ThreadQueue.Count == 0)
          {
            System.Threading.Monitor.Wait(ThreadQueue);
            continue;
          }
          callbackToHandle = ThreadQueue.Peek();
          Assert.IsNotNull(callbackToHandle, "Callback cannot be null");
        }
        callbackToHandle();
        lock (ThreadQueue)
        {
          var dequeuedCallback = ThreadQueue.Dequeue();
          Assert.IsTrue(callbackToHandle == dequeuedCallback, "Queue is in inconsistent state");
        }
      }
      catch (ArgumentException ex)
      {
        Log.Error("CheckThreadLoop() Of ClipboardObserverService produced wrong value", ex, this);
      }
      catch (Exception ex)
      {
        Log.Error("CheckThreadLoop() exception", ex, this);
      }
    }
  }

  protected virtual void InformNewClipboardValue(string newValue)
  {
    List<IPlantEx> enabledPlants = Gardenbed.GetEnabledPlants();
    foreach (IPlantEx enabledPlant in enabledPlants)
    {
      var luggage = GetPlantLuggage(enabledPlant);
      if (luggage != null)
      {
        luggage.InformNewClipboardValue(newValue);
      }
    }
  }

  protected virtual void InitializeClipboardWorkingThread()
  {
    ClipboardWorkingThread = new Thread(CheckThreadLoop) { IsBackground = true };
    ClipboardWorkingThread.SetApartmentState(ApartmentState.STA);
  }

  protected virtual void InitializePlantWithLuggage(IPlantEx plant)
  {
    var asClipboardWorksPerformer = plant.GetFirstWorkhorseOfType<IClipboardWorks>();
    if (asClipboardWorksPerformer != null)
    {
      asClipboardWorksPerformer.StoreClipboardValueProvider(SelfProvider);
    }
    var clipboardListener = plant.GetFirstWorkhorseOfType<IClipboardListener>();
    if (clipboardListener == null)
    {
      return;
    }
    var clipboardObserverPlantBox = new ClipboardObserverPlantBox
    {
      WorksHungry = clipboardListener,
      RelatedPlantEx = plant,
      SettingsBox = plant.MySettingsBox.GetSubBox(LuggageName)
    };
    plant.PutLuggage(LuggageName, clipboardObserverPlantBox);
  }

  /// <summary>
  /// Checks whether the clipboard change event should be fired.
  /// If previous clipboard value is the same, event is ignored.
  /// </summary>
  /// <param name="value"></param>
  /// <returns></returns>
  protected virtual bool IsNewClipboardValue(string value)
  {
    //this is an initial state, no clipboard updates before
    if (lastKnownClipboardLengthOrSpecial == 0)
    {
      return true;
    }
    //check for special values
    if (value.IsNullOrEmpty())
    {
      return lastKnownClipboardLengthOrSpecial == -2 && lastKnownClipboardValue == null;
    }
    if (value.Length > MaxAllowedTextLength)
    {
      return lastKnownClipboardLengthOrSpecial != value.Length;
    }
    return !value.Equals(lastKnownClipboardValue);
  }

  /// <summary>
  /// Method is used to register current clipboard value.
  /// Used to prevent duplicate firings.
  /// </summary>
  /// <param name="value"></param>
  protected virtual void NoteNewClipboardValue(string value)
  {
    //Special case. Special values are set.
    if (value.IsNullOrEmpty())
    {
      lastKnownClipboardLengthOrSpecial = -2;
      lastKnownClipboardValue = null;
      return;
    }
    if (value.Length > MaxAllowedTextLength)
    {
      lastKnownClipboardLengthOrSpecial = value.Length;
      lastKnownClipboardValue = null;
      Assert.IsTrue(lastKnownClipboardLengthOrSpecial != 0, "Zero is reserved value. Cannot be zero");
    }
    else
    {
      lastKnownClipboardLengthOrSpecial = -1;
      lastKnownClipboardValue = value;
    }
  }

  /// <summary>
  /// Method is raised by the Monitor. 
  /// The purpose of the method is to suppress the duplicated "changed" events.
  /// The actual work is performed by the OnClipboardValueChangedHandler method.
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  protected virtual void OnClipboardValueChanged(object sender, EventArgs e)
  {
    //This code starts timer again. If countdown is already started, it's reset
    ClipboardPostponedReactTimer.Change(BufferTimeBeforeReact, Timeout.Infinite);
  }

  /// <summary>
  /// This method performs actual "OnClipboardChanged" event work.
  /// Method is called by the PostponedReactTimer.
  /// </summary>
  /// <param name="dummy"></param>
  protected virtual void OnClipboardValueChangedHandler(object dummy)
  {
    PostToClipboardThread(
      delegate
      {
        var clipboardValue = Clipboard.GetText();
        bool isNewValue = IsNewClipboardValue(clipboardValue);
        NoteNewClipboardValue(clipboardValue);
        if (supressNextEvent)
        {
          supressNextEvent = false;
          return;
        }
        if (!isNewValue)
        {
          return;
        }
        if (clipboardValue.IsNullOrEmpty())
        {
          return;
        }
        if (clipboardValue.Length > MaxAllowedTextLength)
        {
          return;
        }
        InformNewClipboardValue(clipboardValue);
      });
  }

  protected void PostToClipboardThread(Action method)
  {
    lock (ThreadQueue)
    {
      ThreadQueue.Enqueue(method);
      System.Threading.Monitor.Pulse(ThreadQueue);
    }
  }
}