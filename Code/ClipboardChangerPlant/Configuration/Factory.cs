using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClipboardChangerPlant.Configuration
{
  public class Factory
  {
    public static ConfigurationBasedFactory ActualFactory
    {
      get
      {
        return ConfigurationBasedFactory.ActualFactory;
      }
    }
  }
}