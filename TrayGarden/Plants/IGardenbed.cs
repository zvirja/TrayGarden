using System.Collections.Generic;
using JetBrains.Annotations;

namespace TrayGarden.Plants
{
    public interface IGardenbed
    {
        void Initialize(List<object> workhorses);

        List<IPlantInternal> GetAllPlants();
        List<IPlantInternal> GetEnabledPlants();
    }
}