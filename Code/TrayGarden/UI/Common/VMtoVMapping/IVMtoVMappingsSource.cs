#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.UI.Common.VMtoVMapping
{
  public interface IVMtoVMappingsSource
  {
    #region Public Methods and Operators

    List<IViewModelToViewMapping> GetMappings();

    #endregion
  }
}