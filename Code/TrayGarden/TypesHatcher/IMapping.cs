using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Configuration;

namespace TrayGarden.TypesHatcher
{
  public interface IMapping
  {
    Type InterfaceType { get; }

    IObjectFactory ObjectFactory { get; }
  }
}