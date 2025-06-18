using JetBrains.Annotations;

using TrayGarden.Configuration.ApplicationConfiguration.Autorun;
using TrayGarden.UI.Configuration.EntryVM;

namespace TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline;

[UsedImplicitly]
public class AddRunAtStartupSetting
{
  public AddRunAtStartupSetting()
  {
    Description = "Configures whether start the app at the Windows startup";
  }

  public string Description { get; set; }

  [UsedImplicitly]
  public virtual void Process(GetApplicationConfigStepArgs args)
  {
    args.ConfigurationConstructInfo.ConfigurationEntries.Add(GetConfigurationEntry());
  }

  protected virtual ConfigurationEntryBaseVM GetConfigurationEntry()
  {
    var player = new AutorunPlayer("Run at startup", Description);
    return new BoolConfigurationEntryVM(player);
  }
}