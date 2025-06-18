using System;
using System.Drawing;
using ClipboardChangerPlant.Properties;

namespace ClipboardChangerPlant.Configuration;

public class ResourcesOperator
{
  public static Icon GetIconByName(string name)
  {
    return (Icon)Resources.ResourceManager.GetObject(name);
  }

  public static String GetStringByName(string name)
  {
    return Resources.ResourceManager.GetString(name);
  }
}