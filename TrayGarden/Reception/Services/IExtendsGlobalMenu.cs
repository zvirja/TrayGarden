using System;
using System.Drawing;

namespace TrayGarden.Reception.Services
{
  /// <summary>
  /// This service allows to extend the main menu of tray icon.
  /// </summary>
  public interface IExtendsGlobalMenu
  {
    bool GetMenuStripItemData(out string text, out Icon icon, out EventHandler clickHandler);
  }
}
