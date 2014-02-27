#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Diagnostics;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

#endregion

namespace TrayGarden.Services.PlantServices.UserConfig.Core
{
  public class UserSettingMetadata<T> : IUserSettingMetadataMaster<T>
  {
    #region Public Properties

    public virtual object AdditionalParams { get; protected set; }

    public T DefaultValue { get; protected set; }

    public string Description { get; private set; }

    public IUserSettingHallmark Hallmark { get; protected set; }

    public virtual string Name { get; protected set; }

    public virtual string Title { get; protected set; }

    #endregion

    #region Public Methods and Operators

    public void Initialize(
      string name,
      string title,
      T defaultValue,
      string description,
      object additionalParams,
      IUserSettingHallmark hallmark)
    {
      Assert.ArgumentNotNullOrEmpty(name, "name");
      Assert.ArgumentNotNullOrEmpty(title, "title");
      this.Name = name;
      this.Title = title;
      this.DefaultValue = defaultValue;
      this.Description = description;
      this.AdditionalParams = additionalParams;
      this.Hallmark = hallmark;
    }

    #endregion
  }
}