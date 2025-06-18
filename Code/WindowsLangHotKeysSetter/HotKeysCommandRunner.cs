using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;

using TrayGarden.Services.PlantServices.RareCommands.Core;
using TrayGarden.TypesHatcher;
using TrayGarden.UI;

namespace WindowsLangHotKeysSetter;

public class HotKeysCommandRunner : TrayGarden.Reception.Services.IProvidesRareCommands
{
  public static HotKeysCommandRunner Instance = new HotKeysCommandRunner();

  public virtual List<IRareCommand> GetRareCommands()
  {
    var result = new List<IRareCommand>();
    result.Add(GetExecuteCommand());
    return result;
  }

  protected void DisplayResult(string message, bool isError)
  {
    HatcherGuide<IUIManager>.Instance.OKMessageBox("WindowsLangHotKeysSetter", message, isError ? MessageBoxImage.Error : MessageBoxImage.Information);
  }

  protected IRareCommand GetExecuteCommand()
  {
    return new SimpleRareCommand("Set predefined hotkeys", "This command applies hotkeys from configuration (refer to the plant's settings) to Windows.", () => SetHotKeys(true));
  }

  internal bool SetHotKeys(bool displaySuccessDialog)
  {
    List<Tuple<uint, uint, uint, IntPtr>> argsSets = ParamsConfigurator.Instance.GetArgsTuples();
    if (argsSets == null || argsSets.Count == 0)
    {
      DisplayResult("Unable to run because of improper configuration.", true);
      return false;
    }
    bool errorPresent = false;
    foreach (Tuple<uint, uint, uint, IntPtr> argsSet in argsSets)
    {
      var res = CliImmSetHotKey(argsSet.Item1, argsSet.Item2, argsSet.Item3, argsSet.Item4);
      if (res == false)
      {
        errorPresent = true;
      }
    }
    if (errorPresent)
    {
      DisplayResult("Successfully called method for each set, but some of calls returned false result.", true);
      return false;
    }
    if(displaySuccessDialog)
    {
      DisplayResult("Successfully applied all the combinations.", false);
    }

    return true;
  }

  [DllImport("user32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
  private static extern bool CliImmSetHotKey(uint dwID, uint uModifiers, uint uVirtualKey, IntPtr hkl);
}