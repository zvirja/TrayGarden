using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TrayGarden.Reception.Services.StandaloneIcon
{
  /// <summary>
  /// This interface enables the standalone tray icon for plant. The functionality of the item may be extened by INeedToModifyIcon and IExtendContextMenu interfaces
  /// </summary>
  public interface IStandaloneIcon
  {
    bool GetIconInfo(out string title, out Icon icon, out MouseEventHandler iconClickHandler);
  }
}
