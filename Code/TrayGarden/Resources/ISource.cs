#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;

#endregion

namespace TrayGarden.Resources
{
  public interface ISource
  {
    #region Public Properties

    ResourceManager Source { get; }

    #endregion
  }
}