using System.Windows.Input;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;

namespace TrayGarden.UI.Configuration.Stuff
{
  public class ConfigurationEntryForAction : ConfigurationEntryVMBase
  {

    public ICommand Action
    {
      get { return GetSpecificRealPlayer<IConfigurationAwarePlayerWithValues>().Action; }
    }

    public string ActionTitle
    {
      get
      {
        return GetSpecificRealPlayer<IConfigurationAwarePlayerWithValues>().ActionTitle;
      }
    }

    public override bool HideResetButton
    {
      get { return true; }
    }

    public ConfigurationEntryForAction([NotNull] IConfigurationAwarePlayerWithValues realPlayer)
      : base(realPlayer)
    {
      Assert.IsNotNull(realPlayer.Action, "Action cannot be null");
      Assert.IsNotNullOrEmpty(realPlayer.ActionTitle, "Action title cannot be null or empty");
    }
  }
}