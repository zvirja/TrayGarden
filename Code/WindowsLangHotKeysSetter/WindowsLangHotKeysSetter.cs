using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using TrayGarden.Reception;

namespace WindowsLangHotKeysSetter
{
  public class WindowsLangHotKeysSetter : IPlant, TrayGarden.Reception.IServicesDelegation
  {
    public string Description
    {
      get
      {
        return "If you set quick hotkeys for Windows languages switching (e.g. EN - Alt + Shift + 4), sometimes Windows could reset those setting."
          + "This plant restores the combination thorought the WinAPI call of the CliImmSetHotKey method.";
      }
    }

    public string HumanSupportingName
    {
      get
      {
        return "Windows Lang switch hotkeys setter";
      }
    }

    public void Initialize()
    {

    }

    public void PostServicesInitialize()
    {

    }

    public virtual List<object> GetServiceDelegates()
    {
      return new List<object>() { ParamsConfigurator.Instance, HotKeysCommandRunner.Instance };
    }

  }
}
