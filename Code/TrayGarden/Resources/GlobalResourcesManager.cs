#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

#endregion

namespace TrayGarden.Resources
{
  [UsedImplicitly]
  public class GlobalResourcesManager
  {
    #region Public Methods and Operators

    public static Icon GetIconByName(string name)
    {
      return (Icon)GlobalResources.ResourceManager.GetObject(name);
    }

    public static String GetStringByName(string name)
    {
      return GlobalResources.ResourceManager.GetString(name);
    }

    #endregion
  }
}