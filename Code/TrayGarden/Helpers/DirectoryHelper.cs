#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

#endregion

namespace TrayGarden.Helpers
{
  public class DirectoryHelper
  {
    #region Static Fields

    private static string currentDirectory;

    #endregion

    #region Public Properties

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

    #endregion
  }
}