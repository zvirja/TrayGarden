using System;
using System.Collections.Generic;
using JetBrains.Annotations;

using TrayGarden.Configuration;
using TrayGarden.Diagnostics;
using TrayGarden.Helpers;

namespace TrayGarden.TypesHatcher;

[UsedImplicitly]
public class HatcherManager
{
  protected static readonly Lazy<HatcherManager> Instance =
    new Lazy<HatcherManager>(() => Factory.Instance.GetObject<HatcherManager>("typeHatcherManager"));

  public HatcherManager()
  {
    Mappings = new Dictionary<Type, IObjectFactory>();
  }

  public static HatcherManager Actual
  {
    get
    {
      return Instance.Value;
    }
  }

  protected bool Initialized { get; set; }

  protected Dictionary<Type, IObjectFactory> Mappings { get; set; }

  public virtual object GetNewObjectByType(Type keyInterface)
  {
    if (!Initialized)
    {
      throw new NonInitializedException();
    }
    if (Mappings.ContainsKey(keyInterface))
    {
      return Mappings[keyInterface].GetPurelyNewObject();
    }
    Log.For(this).Warning("Hatcher. Can't resolve object {KeyInterface}", keyInterface.FullName);
    return null;
  }

  public virtual object GetObjectByType(Type keyInterface)
  {
    if (!Initialized)
    {
      throw new NonInitializedException();
    }
    if (Mappings.ContainsKey(keyInterface))
    {
      return Mappings[keyInterface].GetObject();
    }
    Log.For(this).Warning("Hatcher. Can't resolve object {KeyInterface}", keyInterface.FullName);
    return null;
  }

  [UsedImplicitly]
  public virtual void Initialize(List<IMapping> mappings)
  {
    Assert.ArgumentNotNull(mappings, "mappings");
    foreach (IMapping mapping in mappings)
    {
      if (ValidateMapping(mapping))
      {
        Mappings.Add(mapping.InterfaceType, mapping.ObjectFactory);
      }
      else
      {
        Log.For(this).Warning("Cannot validate mapping '{Mapping}' for Hatcher", mapping.ToString());
      }
    }
    Initialized = true;
  }

  protected virtual bool ValidateMapping(IMapping mapping)
  {
    Type interfaceType = mapping.InterfaceType;
    if (interfaceType == null)
    {
      return false;
    }
    if (!interfaceType.IsInterface)
    {
      return false;
    }
    if (mapping.ObjectFactory == null)
    {
      return false;
    }
    return true;
  }
}