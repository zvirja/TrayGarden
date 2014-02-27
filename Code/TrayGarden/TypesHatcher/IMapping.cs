#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Configuration;

#endregion

namespace TrayGarden.TypesHatcher
{
  public interface IMapping
  {
    #region Public Properties

    Type InterfaceType { get; }

    IObjectFactory ObjectFactory { get; }

    #endregion
  }
}