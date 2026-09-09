using JetBrains.Annotations;

using TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline;
using TrayGarden.Pipelines.Engine;
using TrayGarden.UI;
using TrayGarden.UI.Common.Commands;
using TrayGarden.UI.Configuration.EntryVM;
using TrayGarden.UI.Configuration.EntryVM.Players;

namespace TrayGarden.DummyTests;

[UsedImplicitly]
public class DummySettingInjection(IUIManager uiManager) : IPipelineProcessor<GetApplicationConfigStepArgs>
{
  public void Process(GetApplicationConfigStepArgs args)
  {
#if (DEBUG)
    args.ConfigurationConstructInfo.ConfigurationEntries.Add(GetActionConfigurationEntry());
#endif
  }

  private ConfigurationEntryBaseVM GetActionConfigurationEntry()
  {
    var realPlayer = new ActionConfigurationPlayer(
      "Dummy setting",
      "Dummy action",
      new RelayCommand(
        delegate(object obj) { uiManager.OKMessageBox("Dummy action", "Dummy action performed"); },
        true));

    return new ActionConfigurationEntry(realPlayer);
  }
}
