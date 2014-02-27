#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden
{
  public class NonInitializedException : Exception
  {
    #region Constructors and Destructors

    public NonInitializedException()
      : base("Object isn't properly initialized. Should be initialized before usage.")
    {
    }

    #endregion
  }
}