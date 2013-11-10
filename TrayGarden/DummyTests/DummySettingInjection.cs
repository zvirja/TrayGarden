using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline;
using TrayGarden.TypesHatcher;
using TrayGarden.UI;
using TrayGarden.UI.Common.Commands;
using TrayGarden.UI.Configuration;
using TrayGarden.UI.Configuration.Stuff;

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

    protected ConfigurationEntryVMBase GetActionConfigurationEntry()
    {
      return new ConfigurationEntryForAction(new ActionAwarePlayer("Dummy setting", "Dummy action", delegate(object obj)
      {
        HatcherGuide<IUIManager>.Instance.OKMessageBox("Dummy action","Dummy action performed");
      }));
    }

    public class ActionAwarePlayer : ConfigurationAwarePlayer
    {
      public ActionAwarePlayer(string settingName, string actionName, Action<object> action)
        : base(settingName, false, false)
      {
        base.ActionTitle = actionName;
        base.Action = new RelayCommand(action, true);
      }
    }
  }
}
