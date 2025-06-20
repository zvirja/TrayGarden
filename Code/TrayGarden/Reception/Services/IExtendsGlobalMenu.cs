﻿using TrayGarden.Services.PlantServices.GlobalMenu.Core.ContextMenuCollecting;

namespace TrayGarden.Reception.Services;

/// <summary>
/// This service allows to extend the main menu of tray icon.
/// </summary>
public interface IExtendsGlobalMenu
{
  bool FillProvidedContextMenuBuilder(IMenuEntriesAppender menuAppender);
}