using JetBrains.Annotations;

using TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline;
using TrayGarden.Pipelines.Engine;
using TrayGarden.UI.Configuration.EntryVM;

namespace TrayGarden.UI.WindowWithReturn.Integration;

[UsedImplicitly]
public class ExitOnCloseSetting : IPipelineProcessor<GetApplicationConfigStepArgs>
{
  public string SettingDescription { get; set; } =
    "If enabled, exit application if window closed, hide if minimized. Otherwise hide when closed.";

  [UsedImplicitly]
  public virtual void Process(GetApplicationConfigStepArgs args)
  {
    args.ConfigurationConstructInfo.ConfigurationEntries.Add(GetConfigurationEntry());
  }

  protected virtual ConfigurationEntryBaseVM GetConfigurationEntry()
  {
    return new BoolConfigurationEntryVM(new ExitOnClosePlayer("Exit on close", SettingDescription));
  }
}
