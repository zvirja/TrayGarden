using JetBrains.Annotations;

using TrayGarden.Configuration.ApplicationConfiguration.Autorun;
using TrayGarden.Pipelines.Engine;
using TrayGarden.UI.Configuration.EntryVM;

namespace TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline;

[UsedImplicitly]
public class AddRunAtStartupSetting(IAutorunHelper autorunHelper) : IPipelineProcessor<GetApplicationConfigStepArgs>
{
  public string Description { get; set; } = "Configures whether start the app at the Windows startup";

  [UsedImplicitly]
  public virtual void Process(GetApplicationConfigStepArgs args)
  {
    args.ConfigurationConstructInfo.ConfigurationEntries.Add(GetConfigurationEntry());
  }

  protected virtual ConfigurationEntryBaseVM GetConfigurationEntry()
  {
    var player = new AutorunPlayer(autorunHelper, "Run at startup", Description);
    return new BoolConfigurationEntryVM(player);
  }
}
