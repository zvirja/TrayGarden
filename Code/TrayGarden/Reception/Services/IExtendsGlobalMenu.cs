#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Services.PlantServices.GlobalMenu.Core.ContextMenuCollecting;

#endregion

namespace TrayGarden.Reception.Services
{
  /// <summary>
  /// This service allows to extend the main menu of tray icon.
  /// </summary>
  public interface IExtendsGlobalMenu
  {
    #region Public Methods and Operators

    bool FillProvidedContextMenuBuilder(IMenuEntriesAppender menuAppender);

    #endregion
  }
}