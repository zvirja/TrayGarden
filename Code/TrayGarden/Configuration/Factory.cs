using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Configuration
{
  public class Factory
  {
    public static IFactory Instance
    {
      get
      {
        return ModernFactory.ActualInstance;
      }
    }
  }
}