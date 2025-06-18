using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Configuration.ApplicationConfiguration.Autorun
{
  public interface IAutorunHelper
  {
    bool IsAddedToAutorun { get; }

    bool SetNewAutorunValue(bool runAtStartup);
  }
}