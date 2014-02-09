#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces.TypeSpecific
{
  public interface IStringOptionUserSetting : ITypedUserSetting<string>
  {
    #region Public Properties

    List<string> PossibleOptions { get; }

    #endregion
  }
}