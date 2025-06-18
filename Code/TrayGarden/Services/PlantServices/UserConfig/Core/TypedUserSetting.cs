using System;
using System.Collections.Generic;
using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;
using TrayGarden.Services.PlantServices.UserConfig.Core.TypeSpecific;

namespace TrayGarden.Services.PlantServices.UserConfig.Core;

public class TypedUserSetting<T> : UserSettingBase, ITypedUserSetting<T>, ITypedUserSettingMaster<T>
{
  protected T currentValue;

  public new event EventHandler<TypedUserSettingChange<T>> ValueChanged;

  public new virtual ITypedUserSettingMetadata<T> Metadata { get; set; }

  public IUserSettingStorage<T> Storage { get; set; }

  public virtual T Value
  {
    get
    {
      AssertInitialized();
      if (!ValueWasAlreadyPooled)
      {
        currentValue = PullValueFromUnderlyingStorage();
        ValueWasAlreadyPooled = true;
      }
      return currentValue;
    }

    set
    {
      AssertInitialized();
      var oldValue = Value;
      currentValue = value;
      PushValueToUnderlyingStorage(value);
      OnValueChanged(oldValue, Value);
    }
  }

  protected bool ValueWasAlreadyPooled { get; set; }

  public virtual void Initialize(
    [NotNull] ITypedUserSettingMetadata<T> typedMetadata,
    [NotNull] IUserSettingStorage<T> storage,
    List<IUserSettingBase> activityCriterias)
  {
    Assert.ArgumentNotNull(typedMetadata, "metadata");
    Assert.ArgumentNotNull(storage, "storage");
    base.Initialize(typedMetadata, activityCriterias);
    Metadata = typedMetadata;
    Storage = storage;
  }

  public override void ResetToDefault()
  {
    AssertInitialized();
    Value = Metadata.DefaultValue;
  }

  protected virtual void OnValueChanged(T oldValue, T newValue)
  {
    var args = new TypedUserSettingChange<T>(this, oldValue, newValue);
    base.OnValueChanged(args);
    EventHandler<TypedUserSettingChange<T>> handler = ValueChanged;
    if (handler != null)
    {
      handler(this, args);
    }
  }

  protected virtual T PullValueFromUnderlyingStorage()
  {
    return Storage.ReadValue(Name, Metadata.DefaultValue);
  }

  protected virtual void PushValueToUnderlyingStorage(T value)
  {
    Storage.WriteValue(Name, value);
  }
}