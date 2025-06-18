using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using TrayGarden.Diagnostics;
using TrayGarden.Plants;
using TrayGarden.Services.PlantServices.StandaloneIcon.Core.InitPlantPipeline;
using TrayGarden.TypesHatcher;

using Application = System.Windows.Application;

namespace TrayGarden.Services.PlantServices.StandaloneIcon.Core
{
  public class StandaloneIconService : PlantServiceBase<StandaloneIconPlantBox>
  {
    public StandaloneIconService()
      : base("Standalone icon", "StandaloneIconService")
    {
      this.ServiceDescription = "Service provides plants with ability to host their own standalone tray icons.";
    }

    public override void InformClosingStage()
    {
      base.InformClosingStage();
      List<IPlantEx> allPlants = HatcherGuide<IGardenbed>.Instance.GetAllPlants();
      foreach (IPlantEx plant in allPlants)
      {
        var siBox = this.GetPlantLuggage(plant);
        if (siBox != null)
        {
          siBox.NotifyIcon.Dispose();
        }
      }
    }

    public override void InformDisplayStage()
    {
      base.InformDisplayStage();
      List<IPlantEx> enabledPlants = HatcherGuide<IGardenbed>.Instance.GetEnabledPlants();
      foreach (IPlantEx enabledPlant in enabledPlants)
      {
        var siBox = this.GetPlantLuggage(enabledPlant);
        if (siBox == null)
        {
          continue;
        }
        siBox.FixNIVisibility();
      }
    }

    public override void InitializePlant(IPlantEx plantEx)
    {
      base.InitializePlant(plantEx);
      this.InitializePlantFromPipeline(plantEx);
    }

    protected void CloseComponentClick(object sender, EventArgs eventArgs)
    {
      var toolStrip = sender as ToolStripItem;
      Assert.IsNotNull(toolStrip, "ToolStripItem expected");
      var siBox = toolStrip.Tag as StandaloneIconPlantBox;
      Assert.IsNotNull(siBox, "StandaloneIconPlantBox expected");
      siBox.IsEnabled = false;
    }

    protected void ExitGardenClick(object sender, EventArgs eventArgs)
    {
      Application.Current.Shutdown();
    }

    protected virtual void InitializePlantFromPipeline(IPlantEx plantEx)
    {
      InitPlantSIPipeline.Run(plantEx, this.LuggageName, this.CloseComponentClick, this.ExitGardenClick);
    }

    protected override void PlantOnEnabledChanged(IPlantEx plantEx, bool newValue)
    {
      var siBox = this.GetPlantLuggage(plantEx);
      if (siBox != null)
      {
        siBox.FixNIVisibility();
      }
    }
  }
}