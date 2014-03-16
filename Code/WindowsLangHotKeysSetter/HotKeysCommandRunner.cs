#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

using TrayGarden.Services.PlantServices.RareCommands.Core;
using TrayGarden.TypesHatcher;
using TrayGarden.UI;

#endregion

namespace WindowsLangHotKeysSetter
{
  public class HotKeysCommandRunner : TrayGarden.Reception.Services.IProvidesRareCommands
  {
    #region Static Fields

    public static HotKeysCommandRunner Instance = new HotKeysCommandRunner();

    #endregion

    #region Public Methods and Operators

    public virtual List<IRareCommand> GetRareCommands()
    {
      var result = new List<IRareCommand>();
      result.Add(this.GetExecuteCommand());
      return result;
    }

    #endregion

    #region Methods

    protected void DisplayResult(string message, bool isError)
    {
      HatcherGuide<IUIManager>.Instance.OKMessageBox("WindowsLangHotKeysSetter", message, isError ? MessageBoxImage.Error : MessageBoxImage.Information);
    }

    protected IRareCommand GetExecuteCommand()
    {
      return new SimpleRareCommand("Set predefined hotkeys", "This command applies hotkeys from configuration (refer to the plant's settings) to Windows.", this.SetHotKeys);
    }

    protected void SetHotKeys()
    {
      List<Tuple<uint, uint, uint, IntPtr>> argsSets = ParamsConfigurator.Instance.GetArgsTuples();
      if (argsSets == null || argsSets.Count == 0)
      {
        this.DisplayResult("Unable to run because of improper configuration.", true);
        return;
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
        this.DisplayResult("Successfully called method for each set, but some of calls returned false result.", true);
      }
      else
      {
        this.DisplayResult("Successfully applied all the combinations.", false);
      }
    }

    [DllImport("user32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
    private static extern bool CliImmSetHotKey(uint dwID, uint uModifiers, uint uVirtualKey, IntPtr hkl);

    #endregion
  }
}