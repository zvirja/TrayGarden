using System;
using System.Drawing;
using JetBrains.Annotations;

namespace TrayGarden.Resources;

[UsedImplicitly]
public class GlobalResourcesManager
{
  public static Icon GetIconByName(string name)
  {
    return (Icon)GlobalResources.ResourceManager.GetObject(name);
  }

  public static String GetStringByName(string name)
  {
    return GlobalResources.ResourceManager.GetString(name);
  }
}