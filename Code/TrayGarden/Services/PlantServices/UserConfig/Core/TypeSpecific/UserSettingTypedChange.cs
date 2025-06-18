using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.TypeSpecific;

public class TypedUserSettingChange<T> : UserSettingBaseChange
{
  public TypedUserSettingChange(IUserSettingBase origin, T oldValue, T newValue)
    : base(origin)
  {
  }

  public T NewValue { get; protected set; }

  public T OldValue { get; protected set; }
}