using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

namespace TrayGarden.Services.PlantServices.UserConfig.Core;

public abstract class UserSettingBase : IUserSettingBase
{
  protected UserSettingBase()
  {
    this.Initialized = false;
  }

  public event EventHandler IsActiveInvalidated;

  public event EventHandler<UserSettingBaseChange> ValueChanged;

  public virtual string Description
  {
    get
    {
      this.AssertInitialized();
      return this.Metadata.Description;
    }
  }

  public virtual bool IsActive
  {
    get
    {
      this.AssertInitialized();
      if (this.ActivityCriterias == null)
      {
        return true;
      }
      return this.ActivityCriterias.All(x => x.IsActive);
    }
  }

  //Be aware that member may be hidden in derived type
  public IUserSettingMetadataBase Metadata { get; set; }

  public virtual string Name
  {
    get
    {
      this.AssertInitialized();
      return this.Metadata.Name;
    }
  }

  public virtual string Title
  {
    get
    {
      this.AssertInitialized();
      return this.Metadata.Title;
    }
  }

  protected List<IUserSettingBase> ActivityCriterias { get; set; }

  protected bool Initialized { get; set; }

  public abstract void ResetToDefault();

  protected virtual void AssertInitialized()
  {
    if (!this.Initialized)
    {
      throw new NonInitializedException();
    }
  }

  protected virtual void Initialize([NotNull] IUserSettingMetadataBase baseMetadata, List<IUserSettingBase> activityCriterias)
  {
    Assert.ArgumentNotNull(baseMetadata, "baseMetadata");
    this.Metadata = baseMetadata;
    this.ActivityCriterias = activityCriterias;
    if (activityCriterias != null)
    {
      foreach (IUserSettingBase activityCriteria in activityCriterias)
      {
        activityCriteria.IsActiveInvalidated += (sender, args) => this.OnIsActiveInvalidated();
      }
    }
    this.Initialized = true;
  }

  protected virtual void OnIsActiveInvalidated()
  {
    EventHandler handler = this.IsActiveInvalidated;
    if (handler != null)
    {
      handler(this, EventArgs.Empty);
    }
  }

  protected virtual void OnValueChanged(UserSettingBaseChange e)
  {
    EventHandler<UserSettingBaseChange> handler = this.ValueChanged;
    if (handler != null)
    {
      handler(this, e);
    }
  }
}