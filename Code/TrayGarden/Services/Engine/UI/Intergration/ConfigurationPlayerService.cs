using TrayGarden.UI.Configuration.EntryVM.Players;

namespace TrayGarden.Services.Engine.UI.Intergration;

public class ConfigurationPlayerService : TypedConfigurationPlayer<bool>
{
  public ConfigurationPlayerService(IService serviceToManage)
    : base(serviceToManage.ServiceName, serviceToManage.CanBeDisabled, !serviceToManage.CanBeDisabled)
  {
    InfoSource = serviceToManage;
    InfoSource.IsEnabledChanged += x => OnValueChanged();
  }

  public IService InfoSource { get; set; }

  public override bool RequiresApplicationReboot
  {
    get
    {
      return InfoSource.IsEnabled != InfoSource.IsActuallyEnabled;
    }
  }

  public override string SettingDescription
  {
    get
    {
      return InfoSource.ServiceDescription;
    }
    protected set
    {
      //Description should be changed. 
    }
  }

  public override bool Value
  {
    get
    {
      return InfoSource.IsEnabled;
    }
    set
    {
      InfoSource.IsEnabled = value;
      OnRequiresApplicationRebootChanged();
    }
  }

  public override void Reset()
  {
    Value = InfoSource.IsActuallyEnabled;
    OnValueChanged();
  }
}