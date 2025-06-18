using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Plants
{
  public interface IGardenbed
  {
    bool AutoDetectPlants { get; set; }

    List<IPlantEx> GetAllPlants();

    List<IPlantEx> GetEnabledPlants();

    void InformPostInitStage();

    void Initialize(List<object> plants);
  }
}