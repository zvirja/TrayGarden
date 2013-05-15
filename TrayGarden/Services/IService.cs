using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Plants;

namespace TrayGarden
{
    public interface IService
    {
        void InitializePlant(IPlant plant);
    }
}
