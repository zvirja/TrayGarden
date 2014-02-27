#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Configuration
{
  public class SimpleObjectFactory : IObjectFactory
  {
    #region Public Properties

    public string ConfigurationPath { get; set; }

    #endregion

    #region Public Methods and Operators

    public virtual object GetObject()
    {
      return Factory.Instance.GetObject(this.ConfigurationPath);
    }

    public virtual object GetPurelyNewObject()
    {
      return Factory.Instance.GetPurelyNewObject(this.ConfigurationPath);
    }

    #endregion
  }
}