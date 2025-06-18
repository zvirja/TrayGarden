using System;
using TrayGarden.Diagnostics;

namespace TrayGarden.Configuration;

public class ResolverBasedObjectFactory : IObjectFactory
{
  public ResolverBasedObjectFactory(Func<object> getInstanceResolver, Func<object> getPurelyNewInstanceResolver)
  {
    GetInstanceResolver = getInstanceResolver;
    GetPurelyNewInstanceResolver = getPurelyNewInstanceResolver;
  }

  public Func<object> GetInstanceResolver { get; protected set; }

  public Func<object> GetPurelyNewInstanceResolver { get; protected set; }

  public virtual object GetObject()
  {
    if (GetInstanceResolver != null)
    {
      return GetInstanceResolver();
    }
    Log.Warn("ResolverBasedObjectFactory GetObject() null returned", this);
    return null;
  }

  public virtual object GetPurelyNewObject()
  {
    if (GetPurelyNewInstanceResolver != null)
    {
      return GetPurelyNewInstanceResolver();
    }
    Log.Warn("ResolverBasedObjectFactory GetPurelyNewObject() null returned", this);
    return null;
  }
}