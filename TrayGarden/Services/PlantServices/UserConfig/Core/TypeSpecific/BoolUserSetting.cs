using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.RuntimeSettings;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces.TypeSpecific;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.TypeSpecific
{
  public class BoolUserSetting : TypedUserSetting<bool>, IBoolUserSetting
  {
    #region Public Properties
    
    public override bool IsActive
    {
      get
      {
        if (!this.Value)
        {
          return false;
        }
        return base.IsActive;
      }
    }

    #endregion

    #region Methods

    public override void Initialize(ITypedUserSettingMetadata<bool> typedMetadata, IUserSettingStorage<bool> storage, List<IUserSettingBase> activityCriterias)
    {
      base.Initialize(typedMetadata, storage, activityCriterias);
      this.ValueChanged += (sender, change) => this.OnIsActiveInvalidated();
    }

    #endregion
  }
}