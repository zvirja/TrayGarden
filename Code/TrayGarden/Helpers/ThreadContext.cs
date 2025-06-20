﻿using System;
using System.Collections;

namespace TrayGarden.Helpers;

public static class ThreadContext
{
  [ThreadStatic]
  private static Hashtable _hashtable;

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
}