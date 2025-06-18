using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Configuration
{
  public class SimpleObjectFactory : IObjectFactory
  {
    public string ConfigurationPath { get; set; }

    public virtual object GetObject()
    {
      return Factory.Instance.GetObject(this.ConfigurationPath);
    }

    public virtual object GetPurelyNewObject()
    {
      return Factory.Instance.GetPurelyNewObject(this.ConfigurationPath);
    }
  }
}