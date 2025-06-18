using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    this.Mappings = new Dictionary<Type, IObjectFactory>();
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
    if (!this.Initialized)
    {
      throw new NonInitializedException();
    }
    if (this.Mappings.ContainsKey(keyInterface))
    {
      return this.Mappings[keyInterface].GetPurelyNewObject();
    }
    Log.Warn("Hatcher. Can't resolve object {0}".FormatWith(keyInterface.FullName), this);
    return null;
  }

  public virtual object GetObjectByType(Type keyInterface)
  {
    if (!this.Initialized)
    {
      throw new NonInitializedException();
    }
    if (this.Mappings.ContainsKey(keyInterface))
    {
      return this.Mappings[keyInterface].GetObject();
    }
    Log.Warn("Hatcher. Can't resolve object {0}".FormatWith(keyInterface.FullName), this);
    return null;
  }

  [UsedImplicitly]
  public virtual void Initialize(List<IMapping> mappings)
  {
    Assert.ArgumentNotNull(mappings, "mappings");
    foreach (IMapping mapping in mappings)
    {
      if (this.ValidateMapping(mapping))
      {
        this.Mappings.Add(mapping.InterfaceType, mapping.ObjectFactory);
      }
      else
      {
        Log.Warn("Cannot validate mapping '{0}' for Hatcher".FormatWith(mapping.ToString()), this);
      }
    }
    this.Initialized = true;
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