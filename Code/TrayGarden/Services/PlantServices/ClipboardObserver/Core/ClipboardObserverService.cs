#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.Plants;
using TrayGarden.Reception.Services;
using TrayGarden.TypesHatcher;

#endregion

namespace TrayGarden.Services.PlantServices.ClipboardObserver.Core
{
  [UsedImplicitly]
  public class ClipboardObserverService : PlantServiceBase<ClipboardObserverPlantBox>
  {
    #region Fields

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

    #endregion

    #region Constructors and Destructors

    public ClipboardObserverService()
      : base("Clipboard Observer", "ClipboardObserverService")
    {
      this.ServiceDescription = "Service monitors the clipboard and deliver change notifications to plants.";
      this.MaxAllowedTextLength = 100000;
      this.supressNextEvent = false;
      this.SelfProvider = new ClipboardProvider(this);
      this.Monitor = new ClipboardMonitor();
      this.Monitor.ClipboardValueChanged += this.OnClipboardValueChanged;
      this.ThreadQueue = new Queue<Action>();
      this.BufferTimeBeforeReact = 500;
      this.ClipboardPostponedReactTimer = new System.Threading.Timer(
        this.OnClipboardValueChangedHandler,
        null,
        Timeout.Infinite,
        Timeout.Infinite);
      //0 is used if this is initial state
      this.lastKnownClipboardLengthOrSpecial = 0;
      this.lastKnownClipboardValue = null;
    }

    #endregion

    //public int CheckIntervalMsec { get; set; }

    #region Public Properties

    /// <summary>
    /// Specifies a time to wait after last clipboard change event. Sometimes we receive a lot of noise from Monitor.
    /// </summary>
    public int BufferTimeBeforeReact { get; set; }

    public int MaxAllowedTextLength { get; set; }

    #endregion

    #region Properties

    protected System.Threading.Timer ClipboardPostponedReactTimer { get; set; }

    protected Thread ClipboardWorkingThread { get; set; }

    protected IGardenbed Gardenbed { get; set; }

    protected ClipboardMonitor Monitor { get; set; }

    protected IClipboardProvider SelfProvider { get; set; }

    protected Queue<Action> ThreadQueue { get; set; }

    #endregion

    #region Public Methods and Operators

    public virtual string GetClipboardValue(bool disableSizeCheck)
    {
      string value = string.Empty;
      var awaiter = new ManualResetEventSlim(false);
      this.PostToClipboardThread(
        delegate
          {
            var clipboardValue = Clipboard.GetText();
            try
            {
              if (clipboardValue.IsNullOrEmpty())
              {
                return;
              }
              if (!disableSizeCheck && clipboardValue.Length > this.MaxAllowedTextLength)
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
      this.ClipboardWorkingThread.Start();
    }

    public override void InformInitializeStage()
    {
      base.InformInitializeStage();
      this.Gardenbed = HatcherGuide<IGardenbed>.Instance;
      this.InitializeClipboardWorkingThread();
    }

    public override void InitializePlant(IPlantEx plantEx)
    {
      base.InitializePlant(plantEx);
      this.InitializePlantWithLuggage(plantEx);
    }

    public virtual void SetClipboardValue(string newValue, bool silent)
    {
      this.PostToClipboardThread(
        delegate
          {
            this.supressNextEvent = silent;
            Clipboard.SetText(newValue);
          });
    }

    #endregion

    #region Methods

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
          lock (this.ThreadQueue)
          {
            if (this.ThreadQueue.Count == 0)
            {
              System.Threading.Monitor.Wait(this.ThreadQueue);
              continue;
            }
            callbackToHandle = this.ThreadQueue.Peek();
            Assert.IsNotNull(callbackToHandle, "Callback cannot be null");
          }
          callbackToHandle();
          lock (this.ThreadQueue)
          {
            var dequeuedCallback = this.ThreadQueue.Dequeue();
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
      List<IPlantEx> enabledPlants = this.Gardenbed.GetEnabledPlants();
      foreach (IPlantEx enabledPlant in enabledPlants)
      {
        var luggage = this.GetPlantLuggage(enabledPlant);
        if (luggage != null)
        {
          luggage.InformNewClipboardValue(newValue);
        }
      }
    }

    protected virtual void InitializeClipboardWorkingThread()
    {
      this.ClipboardWorkingThread = new Thread(this.CheckThreadLoop) { IsBackground = true };
      this.ClipboardWorkingThread.SetApartmentState(ApartmentState.STA);
    }

    protected virtual void InitializePlantWithLuggage(IPlantEx plant)
    {
      var asClipboardWorksPerformer = plant.GetFirstWorkhorseOfType<IClipboardWorks>();
      if (asClipboardWorksPerformer != null)
      {
        asClipboardWorksPerformer.StoreClipboardValueProvider(this.SelfProvider);
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
                                          SettingsBox = plant.MySettingsBox.GetSubBox(this.LuggageName)
                                        };
      plant.PutLuggage(this.LuggageName, clipboardObserverPlantBox);
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
      if (this.lastKnownClipboardLengthOrSpecial == 0)
      {
        return true;
      }
      //check for special values
      if (value.IsNullOrEmpty())
      {
        return this.lastKnownClipboardLengthOrSpecial == -2 && this.lastKnownClipboardValue == null;
      }
      if (value.Length > this.MaxAllowedTextLength)
      {
        return this.lastKnownClipboardLengthOrSpecial != value.Length;
      }
      return !value.Equals(this.lastKnownClipboardValue);
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
        this.lastKnownClipboardLengthOrSpecial = -2;
        this.lastKnownClipboardValue = null;
        return;
      }
      if (value.Length > this.MaxAllowedTextLength)
      {
        this.lastKnownClipboardLengthOrSpecial = value.Length;
        this.lastKnownClipboardValue = null;
        Assert.IsTrue(this.lastKnownClipboardLengthOrSpecial != 0, "Zero is reserved value. Cannot be zero");
      }
      else
      {
        this.lastKnownClipboardLengthOrSpecial = -1;
        this.lastKnownClipboardValue = value;
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
      this.ClipboardPostponedReactTimer.Change(this.BufferTimeBeforeReact, Timeout.Infinite);
    }

    /// <summary>
    /// This method performs actual "OnClipboardChanged" event work.
    /// Method is called by the PostponedReactTimer.
    /// </summary>
    /// <param name="dummy"></param>
    protected virtual void OnClipboardValueChangedHandler(object dummy)
    {
      this.PostToClipboardThread(
        delegate
          {
            var clipboardValue = Clipboard.GetText();
            bool isNewValue = this.IsNewClipboardValue(clipboardValue);
            this.NoteNewClipboardValue(clipboardValue);
            if (this.supressNextEvent)
            {
              this.supressNextEvent = false;
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
            if (clipboardValue.Length > this.MaxAllowedTextLength)
            {
              return;
            }
            this.InformNewClipboardValue(clipboardValue);
          });
    }

    protected void PostToClipboardThread(Action method)
    {
      lock (this.ThreadQueue)
      {
        this.ThreadQueue.Enqueue(method);
        System.Threading.Monitor.Pulse(this.ThreadQueue);
      }
    }

    #endregion
  }
}