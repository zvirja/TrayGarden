using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Helpers;

public static class ReflectionHelper
{
  public static Type ResolveType(string typeName)
  {
    Type resolved = Type.GetType(typeName, false);
    if (resolved != null)
    {
      return resolved;
    }
    foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
    {
      resolved = assembly.GetType(typeName, false);
      if (resolved != null)
      {
        break;
      }
    }
    return resolved;
  }
}