using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using JetBrains.Annotations;
using TrayGarden.Plants;
using TrayGarden.Reception.Services;
using TrayGarden.TypesHatcher;
using Timer = System.Timers.Timer;

namespace TrayGarden.Services.PlantServices.ClipboardObserver.Core
{
  [UsedImplicitly]
  public class ClipboardObserverService : PlantServiceBase<ClipboardObserverPlantBox>
  {
    protected volatile string lastCheckClipboardValue;
    protected volatile string queuedValueToSet;
    protected volatile bool queueingSilentMode;

    public int CheckIntervalMsec { get; set; }
    protected Thread CheckThread { get; set; }
    protected IGardenbed Gardenbed { get; set; }
    protected bool IsNeedInService { get; set; }
    protected IClipboardProvider SelfProvider { get; set; }

    public ClipboardObserverService()
      : base("Clipboard Observer", "ClipboardObserverService")
    {
      ServiceDescription = "Service monitors the clipboard and deliver change notifications to plants.";
      CheckIntervalMsec = 200;
      IsNeedInService = false;
      queuedValueToSet = null;
      SelfProvider = new ClipboardProvider(this);
    }

    public override void InformInitializeStage()
    {
      base.InformInitializeStage();
      Gardenbed = HatcherGuide<IGardenbed>.Instance;
      InitializeCheckThread();
      lastCheckClipboardValue = Clipboard.GetText();
    }

    public override void InformDisplayStage()
    {
      base.InformDisplayStage();
      if (IsNeedInService)
        CheckThread.Start();
    }

    public override void InitializePlant(IPlantEx plantEx)
    {
      base.InitializePlant(plantEx);
      InitializePlantWithLuggage(plantEx);
    }

    public virtual void QueueNewClipboardValue(string newValue, bool silent)
    {
      queuedValueToSet = newValue;
      queueingSilentMode = silent;
    }

    public virtual string GetLastTimeClipboardValue()
    {
      return lastCheckClipboardValue;
    }

    protected virtual void InitializeCheckThread()
    {
      CheckThread = new Thread(CheckThreadLoop)
          {
            IsBackground = true
          };
      CheckThread.SetApartmentState(ApartmentState.STA);
    }

    protected virtual void CheckThreadLoop()
    {
      do
      {
        if (queuedValueToSet != null)
        {
          Clipboard.SetText(queuedValueToSet);
          if (queueingSilentMode)
            lastCheckClipboardValue = queuedValueToSet;
          queueingSilentMode = false;
          queuedValueToSet = null;
        }
        //FIX FROM CLIPBRD_E_CANT_OPEN
        try
        {
          string newClipboardValue = Clipboard.GetText();
          if (!newClipboardValue.Equals(lastCheckClipboardValue))
          {
            InformNewClipboardValue(newClipboardValue);
            lastCheckClipboardValue = newClipboardValue;
          }
        }
        catch
        {
        }
        Thread.Sleep(CheckIntervalMsec);
      } while (IsNeedInService);
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

    protected virtual void InitializePlantWithLuggage(IPlantEx plant)
    {
      var asExpected = plant.GetFirstWorkhorseOfType<IClipboardWorks>();
      if (asExpected == null)
        return;
      IsNeedInService = true;
      asExpected.StoreClipboardValueProvider(SelfProvider);
      var clipboardObserverPlantBox = new ClipboardObserverPlantBox
          {
            WorksHungry = asExpected,
            RelatedPlantEx = plant,
            SettingsBox = plant.MySettingsBox.GetSubBox(LuggageName)
          };
      plant.PutLuggage(LuggageName, clipboardObserverPlantBox);
    }

  }
}