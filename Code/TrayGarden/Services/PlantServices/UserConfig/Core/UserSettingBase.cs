using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

namespace TrayGarden.Services.PlantServices.UserConfig.Core;

public abstract class UserSettingBase : IUserSettingBase
{
  protected UserSettingBase()
  {
    Initialized = false;
  }

  public event EventHandler IsActiveInvalidated;

  public event EventHandler<UserSettingBaseChange> ValueChanged;

  public virtual string Description
  {
    get
    {
      AssertInitialized();
      return Metadata.Description;
    }
  }

  public virtual bool IsActive
  {
    get
    {
      AssertInitialized();
      if (ActivityCriterias == null)
      {
        return true;
      }
      return ActivityCriterias.All(x => x.IsActive);
    }
  }

  //Be aware that member may be hidden in derived type
  public IUserSettingMetadataBase Metadata { get; set; }

  public virtual string Name
  {
    get
    {
      AssertInitialized();
      return Metadata.Name;
    }
  }

  public virtual string Title
  {
    get
    {
      AssertInitialized();
      return Metadata.Title;
    }
  }

  protected List<IUserSettingBase> ActivityCriterias { get; set; }

  protected bool Initialized { get; set; }

  public abstract void ResetToDefault();

  protected virtual void AssertInitialized()
  {
    if (!Initialized)
    {
      throw new NonInitializedException();
    }
  }

  protected virtual void Initialize([NotNull] IUserSettingMetadataBase baseMetadata, List<IUserSettingBase> activityCriterias)
  {
    Assert.ArgumentNotNull(baseMetadata, "baseMetadata");
    Metadata = baseMetadata;
    ActivityCriterias = activityCriterias;
    if (activityCriterias != null)
    {
      foreach (IUserSettingBase activityCriteria in activityCriterias)
      {
        activityCriteria.IsActiveInvalidated += (sender, args) => OnIsActiveInvalidated();
      }
    }
    Initialized = true;
  }

  protected virtual void OnIsActiveInvalidated()
  {
    EventHandler handler = IsActiveInvalidated;
    if (handler != null)
    {
      handler(this, EventArgs.Empty);
    }
  }

  protected virtual void OnValueChanged(UserSettingBaseChange e)
  {
    EventHandler<UserSettingBaseChange> handler = ValueChanged;
    if (handler != null)
    {
      handler(this, e);
    }
  }
}