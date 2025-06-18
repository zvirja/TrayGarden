using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Diagnostics;

namespace TrayGarden.Configuration
{
  public class ResolverBasedObjectFactory : IObjectFactory
  {
    public ResolverBasedObjectFactory(Func<object> getInstanceResolver, Func<object> getPurelyNewInstanceResolver)
    {
      this.GetInstanceResolver = getInstanceResolver;
      this.GetPurelyNewInstanceResolver = getPurelyNewInstanceResolver;
    }

    public Func<object> GetInstanceResolver { get; protected set; }

    public Func<object> GetPurelyNewInstanceResolver { get; protected set; }

    public virtual object GetObject()
    {
      if (this.GetInstanceResolver != null)
      {
        return this.GetInstanceResolver();
      }
      Log.Warn("ResolverBasedObjectFactory GetObject() null returned", this);
      return null;
    }

    public virtual object GetPurelyNewObject()
    {
      if (this.GetPurelyNewInstanceResolver != null)
      {
        return this.GetPurelyNewInstanceResolver();
      }
      Log.Warn("ResolverBasedObjectFactory GetPurelyNewObject() null returned", this);
      return null;
    }
  }
}