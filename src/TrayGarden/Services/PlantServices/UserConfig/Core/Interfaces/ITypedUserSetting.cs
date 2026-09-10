using System;
using TrayGarden.Services.PlantServices.UserConfig.Core.TypeSpecific;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

public interface ITypedUserSetting<T> : IUserSettingBase
{
  new event EventHandler<TypedUserSettingChange<T>> ValueChanged;

  new ITypedUserSettingMetadata<T> Metadata { get; set; }

  IUserSettingStorage<T> Storage { get; }

  T Value { get; set; }
}