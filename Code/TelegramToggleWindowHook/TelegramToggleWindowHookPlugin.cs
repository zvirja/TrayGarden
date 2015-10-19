#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Reception;

#endregion

namespace TelegramToggleWindowHook
{
  public class TelegramToggleWindowHookPlant : IPlant
  {
    #region Fields

    private KeyboardHookProcessingForm _form;

    #endregion

    #region Interface Impl

    public string Description
    {
      get { return "Plugin hooks the Ctrl+` shortcut and toggles the Telegram window state\nIs active even if you see that it's disabled here."; }
    }

    public string HumanSupportingName
    {
      get { return "Telegram main window toggle by shortcut"; }
    }

    public void Initialize()
    {
      this._form = new KeyboardHookProcessingForm();
    }

    public void PostServicesInitialize()
    {
    }

    #endregion
  }
}