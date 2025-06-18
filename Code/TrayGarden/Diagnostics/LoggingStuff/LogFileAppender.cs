using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using log4net.Appender;

using TrayGarden.Helpers;

namespace TrayGarden.Diagnostics.LoggingStuff
{
  [UsedImplicitly]
  public class LogFileAppender : FileAppender
  {
    protected static string DirectoryName = ResolveDirectoryName();

    public override string File
    {
      get
      {
        return GetLogFileNameFull();
      }
      set
      {
        base.File = GetLogFileNameFull();
      }
    }

    protected static string GetLogFileName()
    {
      return AppConfigHelper.GetBoolSetting("Log4net.UseSingleLogFile", true)
               ? "Log.log"
               : "Log " + (DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".log");
    }

    protected static string GetLogFileNameFull()
    {
      var directory = ResolveDirectoryPath();
      Directory.CreateDirectory(directory);
      return Path.Combine(directory, GetLogFileName());
    }

    protected static string ResolveDirectoryName()
    {
      var configuredValue = ConfigurationManager.AppSettings["Log4net.FolderNameInAppData"];
      return configuredValue.NotNullNotEmpty() ? configuredValue : "TrayGarden";
    }

    protected static string ResolveDirectoryPath()
    {
      if (!AppConfigHelper.GetBoolSetting("Log4net.UseAppData", false))
      {
        return DirectoryHelper.CurrentDirectory;
      }
      string directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DirectoryName);
      directory = Path.Combine(directory, "Logs");
      return directory;
    }
  }
}