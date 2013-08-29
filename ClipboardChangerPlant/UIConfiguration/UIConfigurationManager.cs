using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Reception.Services;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

namespace ClipboardChangerPlant.UIConfiguration
{
  public class UIConfigurationManager : IUserConfiguration
  {
    #region Static

    public static UIConfigurationManager ActualManager { get; protected set; }

    static UIConfigurationManager()
    {
      ActualManager = new UIConfigurationManager();
    }

    #endregion

    public IUserSettingsBridge UserSettingsBridge { get; set; }
    public List<Action<IUserSettingsMetadataBuilder>> VolatileUserSettings { get; protected set; }
    public Dictionary<string, IUserSetting> UserSettings { get; set; }

    public UIConfigurationManager()
    {
      VolatileUserSettings = new List<Action<IUserSettingsMetadataBuilder>>();
    }


    public virtual bool GetUserSettingsMetadata(IUserSettingsMetadataBuilder metadataBuilder)
    {
      AddNativeSettings(metadataBuilder);
      AddVolatileSettings(metadataBuilder);
      return true;
    }

    public virtual void StoreUserSettingsBridge(IUserSettingsBridge userSettingsBridge)
    {
      UserSettingsBridge = userSettingsBridge;
      InitializeUserSettingsDictionary();
    }

    protected virtual void AddNativeSettings(IUserSettingsMetadataBuilder metadataBuilder)
    {
    }

    protected virtual void AddVolatileSettings(IUserSettingsMetadataBuilder metadataBuilder)
    {
      foreach (Action<IUserSettingsMetadataBuilder> volatileUserSettingInjector in VolatileUserSettings)
      {
        volatileUserSettingInjector(metadataBuilder);
      }
    }

    protected virtual void InitializeUserSettingsDictionary()
    {
      UserSettings = UserSettingsBridge.GetUserSettings().ToDictionary(x => x.Name);
    }
  }
}
