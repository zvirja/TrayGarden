#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.TypesHatcher;
using TrayGarden.UI.Configuration.EntryVM.Players;

#endregion

namespace TrayGarden.Plants.Intergration
{
  public class AutoLoadPropertyPlayer : TypedConfigurationPlayer<bool>
  {
    #region Constructors and Destructors

    public AutoLoadPropertyPlayer([NotNull] string settingName, string settingDescription)
      : base(settingName, false, false)
    {
      base.SettingDescription = settingDescription;
    }

    #endregion

    #region Public Properties

    public override bool Value
    {
      get
      {
        return HatcherGuide<IGardenbed>.Instance.AutoDetectPlants;
      }
      set
      {
        HatcherGuide<IGardenbed>.Instance.AutoDetectPlants = value;
      }
    }

    #endregion

    #region Public Methods and Operators

    public override void Reset()
    {
    }

    #endregion
  }
}