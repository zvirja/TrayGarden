using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;
using TrayGarden.UI.Configuration.EntryVM.Players;

namespace TrayGarden.Services.PlantServices.UserConfig.UI.UserSettingPlayers;

public class TypedUserSettingPlayer<TValue> : TypedConfigurationPlayer<TValue>
{
  public TypedUserSettingPlayer([NotNull] ITypedUserSetting<TValue> userSetting)
    : base(userSetting.Title, true, false)
  {
    Assert.ArgumentNotNull(userSetting, "userSetting");
    UserSetting = userSetting;
    UserSetting.ValueChanged += (before, after) => OnValueChanged();
  }

  public override string SettingDescription
  {
    get
    {
      return UserSetting.Description;
    }
    protected set
    {
    }
  }

  public ITypedUserSetting<TValue> UserSetting { get; set; }

  public override TValue Value
  {
    get
    {
      return UserSetting.Value;
    }
    set
    {
      UserSetting.Value = value;
    }
  }

  public override void Reset()
  {
    UserSetting.ResetToDefault();
  }
}