using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Plants
{
    public delegate void PlantEnabledChangedEvent(IPlantInternal plantInternal, bool newValue);
}
