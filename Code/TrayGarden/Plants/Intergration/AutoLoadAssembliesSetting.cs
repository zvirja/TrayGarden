using JetBrains.Annotations;

using TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline;
using TrayGarden.Pipelines.Engine;
using TrayGarden.UI.Configuration.EntryVM;

namespace TrayGarden.Plants.Intergration;

public class AutoLoadAssembliesSetting(IGardenbed gardenbed) : IPipelineProcessor<GetApplicationConfigStepArgs>
{
  public string SettingDescription { get; set; } =
    "If this setting is enabled, Tray Garden automatically meets with plants in assemblies. The lookup folder is specified in the appsettings.json file.";

  [UsedImplicitly]
  public virtual void Process(GetApplicationConfigStepArgs args)
  {
    args.ConfigurationConstructInfo.ConfigurationEntries.Add(GetConfigurationEntry());
  }

  protected virtual ConfigurationEntryBaseVM GetConfigurationEntry()
  {
    return new BoolConfigurationEntryVM(new AutoLoadPropertyPlayer(gardenbed, "Auto load plants", SettingDescription));
  }
}
