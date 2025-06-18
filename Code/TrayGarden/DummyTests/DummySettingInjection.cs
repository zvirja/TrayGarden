using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline;
using TrayGarden.TypesHatcher;
using TrayGarden.UI;
using TrayGarden.UI.Common.Commands;
using TrayGarden.UI.Configuration.EntryVM;
using TrayGarden.UI.Configuration.EntryVM.Players;

namespace TrayGarden.DummyTests;

[UsedImplicitly]
public class DummySettingInjection
{
  public virtual void Process(GetApplicationConfigStepArgs args)
  {
#if(DEBUG)
      args.ConfigurationConstructInfo.ConfigurationEntries.Add(this.GetActionConfigurationEntry());
#endif
  }

  protected ConfigurationEntryBaseVM GetActionConfigurationEntry()
  {
    var realPlayer = new ActionConfigurationPlayer(
      "Dummy setting",
      "Dummy action",
      new RelayCommand(
        delegate(object obj) { HatcherGuide<IUIManager>.Instance.OKMessageBox("Dummy action", "Dummy action performed"); },
        true));

    return new ActionConfigurationEntry(realPlayer);
  }
}