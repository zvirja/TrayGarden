using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.UI.Common.VMtoVMapping
{
    public interface IVMtoVMappingsSource
    {
        List<IViewModelToViewMapping> GetMappings();
    }
}
