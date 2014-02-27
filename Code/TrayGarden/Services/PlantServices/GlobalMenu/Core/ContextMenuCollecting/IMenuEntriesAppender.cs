#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.ContextMenuCollecting
{
  public interface IMenuEntriesAppender
  {
    #region Public Methods and Operators

    void AppentMenuStripItem(string text, Icon icon, EventHandler clickHandler);

    #endregion
  }
}