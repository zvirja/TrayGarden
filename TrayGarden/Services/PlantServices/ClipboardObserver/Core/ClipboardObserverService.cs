using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.Plants;
using TrayGarden.Reception.Services;
using TrayGarden.TypesHatcher;
using Timer = System.Timers.Timer;

namespace TrayGarden.Services.PlantServices.ClipboardObserver.Core
{
  [UsedImplicitly]
  public class ClipboardObserverService : PlantServiceBase<ClipboardObserverPlantBox>
  {
    protected volatile bool supressNextEvent;
    protected object timerLock = new object();

    //public int CheckIntervalMsec { get; set; }
    public int MaxAllowedTextLength { get; set; }
    /// <summary>
    /// Specifies a time to wait after last clipboard change event. Sometimes we receive a lot of noise from Monitor.
    /// </summary>
    public int BufferTimeBeforeReact { get; set; }
    protected Thread ClipboardWorkingThread { get; set; }
    protected IGardenbed Gardenbed { get; set; }
    protected IClipboardProvider SelfProvider { get; set; }
    protected ClipboardMonitor Monitor { get; set; }
    protected Queue<Action> ThreadQueue { get; set; }
    protected System.Threading.Timer ClipboardPostponedReactTimer { get; set; }

    public ClipboardObserverService()
      : base("Clipboard Observer", "ClipboardObserverService")
    {
      ServiceDescription = "Service monitors the clipboard and deliver change notifications to plants.";
      MaxAllowedTextLength = 10000;
      supressNextEvent = false;
      SelfProvider = new ClipboardProvider(this);
      Monitor = new ClipboardMonitor();
      Monitor.ClipboardValueChanged += OnClipboardValueChanged;
      ThreadQueue = new Queue<Action>();
      BufferTimeBeforeReact = 500;
      ClipboardPostponedReactTimer = new System.Threading.Timer(OnClipboardValueChangedHandler, null, Timeout.Infinite,
        Timeout.Infinite);
    }

    public override void InformInitializeStage()
    {
      base.InformInitializeStage();
      Gardenbed = HatcherGuide<IGardenbed>.Instance;
      InitializeClipboardWorkingThread();
    }

    public override void InformDisplayStage()
    {
      base.InformDisplayStage();
      ClipboardWorkingThread.Start();
    }

    public override void InitializePlant(IPlantEx plantEx)
    {
      base.InitializePlant(plantEx);
      InitializePlantWithLuggage(plantEx);
    }

    public virtual void SetClipboardValue(string newValue, bool silent)
    {
      supressNextEvent = silent;
      PostToClipboardThread(() => Clipboard.SetText(newValue));
    }

    public virtual string GetClipboardValue(bool disableSizeCheck)
    {
      string value = string.Empty;
      var awaiter = new ManualResetEventSlim(false);
      PostToClipboardThread(delegate
      {
        var clipboardValue = Clipboard.GetText();
        try
        {
          if (clipboardValue.IsNullOrEmpty())
            return;
          if (!disableSizeCheck && clipboardValue.Length > MaxAllowedTextLength)
            return;
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

    protected virtual void InitializeClipboardWorkingThread()
    {
      ClipboardWorkingThread = new Thread(CheckThreadLoop)
          {
            IsBackground = true
          };
      ClipboardWorkingThread.SetApartmentState(ApartmentState.STA);
    }

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
        catch
        {

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
          luggage.InformNewClipboardValue(newValue);
      }
    }

    protected virtual void OnClipboardValueChanged(object sender, EventArgs e)
    {
      //This code starts timer again. If countdown is already started, it's reset
      ClipboardPostponedReactTimer.Change(BufferTimeBeforeReact, Timeout.Infinite);
    }

    protected virtual void OnClipboardValueChangedHandler(object dummy)
    {
      if (supressNextEvent)
      {
        supressNextEvent = false;
        return;
      }
      PostToClipboardThread(delegate
      {
        var clipboardValue = Clipboard.GetText();
        if (clipboardValue.IsNullOrEmpty())
          return;
        if (clipboardValue.Length > MaxAllowedTextLength)
          return;
        InformNewClipboardValue(clipboardValue);
      });
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
        return;
      var clipboardObserverPlantBox = new ClipboardObserverPlantBox
          {
            WorksHungry = clipboardListener,
            RelatedPlantEx = plant,
            SettingsBox = plant.MySettingsBox.GetSubBox(LuggageName)
          };
      plant.PutLuggage(LuggageName, clipboardObserverPlantBox);
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
}