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
      this.AssertInitialized();
      if (!this.ValueWasAlreadyPooled)
      {
        this.currentValue = this.PullValueFromUnderlyingStorage();
        this.ValueWasAlreadyPooled = true;
      }
      return this.currentValue;
    }

    set
    {
      this.AssertInitialized();
      var oldValue = this.Value;
      this.currentValue = value;
      this.PushValueToUnderlyingStorage(value);
      this.OnValueChanged(oldValue, this.Value);
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
    this.Metadata = typedMetadata;
    this.Storage = storage;
  }

  public override void ResetToDefault()
  {
    this.AssertInitialized();
    this.Value = this.Metadata.DefaultValue;
  }

  protected virtual void OnValueChanged(T oldValue, T newValue)
  {
    var args = new TypedUserSettingChange<T>(this, oldValue, newValue);
    base.OnValueChanged(args);
    EventHandler<TypedUserSettingChange<T>> handler = this.ValueChanged;
    if (handler != null)
    {
      handler(this, args);
    }
  }

  protected virtual T PullValueFromUnderlyingStorage()
  {
    return this.Storage.ReadValue(this.Name, this.Metadata.DefaultValue);
  }

  protected virtual void PushValueToUnderlyingStorage(T value)
  {
    this.Storage.WriteValue(this.Name, value);
  }
}