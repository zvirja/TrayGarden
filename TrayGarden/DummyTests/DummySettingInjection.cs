using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline;
using TrayGarden.Helpers;
using TrayGarden.Resources;
using TrayGarden.TypesHatcher;
using TrayGarden.UI;
using TrayGarden.UI.Common.Commands;
using TrayGarden.UI.Configuration;
using TrayGarden.UI.Configuration.EntryVM;
using TrayGarden.UI.Configuration.EntryVM.Players;

namespace TrayGarden.DummyTests
{
  [UsedImplicitly]
  public class DummySettingInjection
  {



    public virtual void Process(GetApplicationConfigStepArgs args)
    {
#if(DEBUG)
      args.ConfigurationConstructInfo.ConfigurationEntries.Add(GetActionConfigurationEntry());
#endif
    }

    protected ConfigurationEntryBaseVM GetActionConfigurationEntry()
    {
      var realPlayer = new ActionConfigurationPlayer("Dummy setting", "Dummy action", new RelayCommand(delegate(object obj)
      {
        HatcherGuide<IUIManager>.Instance.OKMessageBox("Dummy action", "Dummy action performed");
      }, true));

      return new ActionConfigurationEntry(realPlayer);
    }
  }
}
