﻿using System;
using JetBrains.Annotations;

using TrayGarden.Configuration;
using TrayGarden.Diagnostics;
using TrayGarden.Helpers;

namespace TrayGarden.TypesHatcher;

[UsedImplicitly]
public class Mapping : IMapping
{
  public Type InterfaceType { get; protected set; }

  public IObjectFactory ObjectFactory { get; protected set; }

  [UsedImplicitly]
  public virtual void Initialize([NotNull] Type interfaceType, IObjectFactory objectFactory)
  {
    Assert.ArgumentNotNull(interfaceType, "interfaceType");
    Assert.ArgumentNotNull(objectFactory, "objectFactory");
    InterfaceType = interfaceType;
    ObjectFactory = objectFactory;
  }

  public override string ToString()
  {
    if (ObjectFactory == null || InterfaceType == null)
    {
      return base.ToString();
    }
    return "Mapping: Interface {0}, ObjFactory {1}".FormatWith(InterfaceType.FullName, ObjectFactory.GetType().FullName);
  }
}