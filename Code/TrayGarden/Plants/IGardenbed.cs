#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Plants
{
  public interface IGardenbed
  {
    #region Public Properties

    bool AutoDetectPlants { get; set; }

    #endregion

    #region Public Methods and Operators

    List<IPlantEx> GetAllPlants();

    List<IPlantEx> GetEnabledPlants();

    void InformPostInitStage();

    void Initialize(List<object> plants);

    #endregion
  }
}