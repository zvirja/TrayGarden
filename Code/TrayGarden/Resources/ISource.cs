using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;

namespace TrayGarden.Resources
{
  public interface ISource
  {
    ResourceManager Source { get; }
  }
}