using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Plants;
using TrayGarden.Reception.Services;
using TrayGarden.RuntimeSettings;
using TrayGarden.Services.PlantServices.ClipboardObserver.Core;

namespace TrayGarden.Services.PlantServices.CustomSettings.Core;

[UsedImplicitly]
public class CustomSettingsService : PlantServiceBase<ClipboardObserverPlantBox>
{
  public CustomSettingsService()
    : base("Custom settings", "CustomSettingsService")
  {
    this.ServiceDescription = "Service provides plants with settings storage. For plant internal usage.";
  }

  public override void InitializePlant(IPlantEx plantEx)
  {
    this.SetCustomSettingsBox(plantEx);
  }

  protected virtual void SetCustomSettingsBox(IPlantEx plantEx)
  {
    var asExpected = plantEx.GetFirstWorkhorseOfType<IGetCustomSettingsStorage>();
    if (asExpected == null)
    {
      return;
    }
    ISettingsBox settingsBox = plantEx.MySettingsBox.GetSubBox(this.LuggageName);
    asExpected.StoreCustomSettingsStorage(settingsBox);

    //Store luggage
    var luggage = new CustomSettingsServicePlantBox { RelatedPlantEx = plantEx, SettingsBox = settingsBox, IsEnabled = true, };
    plantEx.PutLuggage(this.LuggageName, luggage);
  }
}