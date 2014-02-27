#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Helpers
{
  public static class ThreadContext
  {
    #region Static Fields

    [ThreadStatic]
    private static Hashtable _hashtable;

    #endregion

    #region Public Properties

    public static Hashtable ContextItems
    {
      get
      {
        if (_hashtable != null)
        {
          return _hashtable;
        }
        _hashtable = new Hashtable();
        return _hashtable;
      }
    }

    #endregion
  }
}