#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Helpers
{
  public static class ReflectionHelper
  {
    #region Public Methods and Operators

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

    #endregion
  }
}