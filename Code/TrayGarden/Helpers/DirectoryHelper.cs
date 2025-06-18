using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TrayGarden.Helpers;

public class DirectoryHelper
{
  private static string currentDirectory;

  public static string CurrentDirectory
  {
    get
    {
      if (currentDirectory != null)
      {
        return currentDirectory;
      }
      currentDirectory = new FileInfo(Assembly.GetEntryAssembly().Location).DirectoryName;
      return currentDirectory;
    }
  }
}