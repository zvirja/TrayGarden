using System;
using System.Drawing;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.DynamicState;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.ContextMenuCollecting;

public interface IMenuEntriesAppender
{
  void AppentMenuStripItem(string text, Icon icon, EventHandler clickHandler, IDynamicStateProvider dynamicStateProvider = null);
}