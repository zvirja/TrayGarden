using JetBrains.Annotations;

using TrayGarden.RuntimeSettings;

namespace TrayGarden.Services.PlantServices.StandaloneIcon.Core.InitPlantPipeline;

[UsedImplicitly]
public class ResolveSettingsBox
{
  [UsedImplicitly]
  public virtual void Process(InitPlantSIArgs args)
  {
    ISettingsBox settingsBox = args.SIBox.RelatedPlantEx.MySettingsBox.GetSubBox("StandaloneIconService");
    args.SIBox.SettingsBox = settingsBox;
  }
}