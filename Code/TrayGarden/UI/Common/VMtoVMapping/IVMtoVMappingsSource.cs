using System.Collections.Generic;

namespace TrayGarden.UI.Common.VMtoVMapping;

public interface IVMtoVMappingsSource
{
  List<IViewModelToViewMapping> GetMappings();
}