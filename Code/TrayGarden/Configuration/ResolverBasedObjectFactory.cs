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
    Log.For(this).Warning("ResolverBasedObjectFactory GetObject() null returned");
    return null;
  }

  public virtual object GetPurelyNewObject()
  {
    if (GetPurelyNewInstanceResolver != null)
    {
      return GetPurelyNewInstanceResolver();
    }
    Log.For(this).Warning("ResolverBasedObjectFactory GetPurelyNewObject() null returned");
    return null;
  }
}