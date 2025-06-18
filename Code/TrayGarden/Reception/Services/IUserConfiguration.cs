using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

namespace TrayGarden.Reception.Services
{
  /// <summary>
  /// This service allows plant to declare the user settings. User may change these settings using UI.
  /// </summary>
  public interface IUserConfiguration
  {
    /// <summary>
    /// This method allows plant to store and use personal settings steward.
    /// Steward should be used to declare user settings.
    /// Settings might be declared at any point of program execution.
    /// </summary>
    /// <param name="personalSettingsSteward"></param>
    void StoreAndFillPersonalSettingsSteward(IPersonalUserSettingsSteward personalSettingsSteward);
  }
}