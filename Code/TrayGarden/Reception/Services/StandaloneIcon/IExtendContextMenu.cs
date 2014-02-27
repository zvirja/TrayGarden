#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

#endregion

namespace TrayGarden.Reception.Services.StandaloneIcon
{
  /// <summary>
  /// This interface allows to extend the context menu created by IStandaloneIcon.
  /// Pay attention that object may implement IStandaloneIcon to get it work.
  /// </summary>
  public interface IExtendContextMenu
  {
    #region Public Methods and Operators

    List<ToolStripMenuItem> GetStripsToAdd();

    #endregion
  }
}