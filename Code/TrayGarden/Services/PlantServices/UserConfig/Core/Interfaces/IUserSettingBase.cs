using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces
{
  public interface IUserSettingBase
  {
    event EventHandler IsActiveInvalidated;

    event EventHandler<UserSettingBaseChange> ValueChanged;

    string Description { get; }

    /// <summary>
    /// Is used for logical setting activity. For instance, volume is inactive if sound setting is off.
    /// </summary>
    bool IsActive { get; }

    IUserSettingMetadataBase Metadata { get; }

    string Name { get; }

    string Title { get; }

    void ResetToDefault();
  }
}