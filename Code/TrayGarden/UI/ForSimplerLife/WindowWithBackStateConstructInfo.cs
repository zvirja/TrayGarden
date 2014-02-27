#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.UI.Common.Commands;
using TrayGarden.UI.WindowWithReturn;

#endregion

namespace TrayGarden.UI.ForSimplerLife
{
  public class WindowWithBackStateConstructInfo
  {
    #region Public Properties

    public object ContentVM { get; set; }

    public string GlobalTitle { get; set; }

    public string Header { get; set; }

    public WindowStepState ResultState { get; set; }

    public string ShortName { get; set; }

    public List<ActionCommandVM> StateSpecificHelpActions { get; set; }

    public ActionCommandVM SuperAction { get; set; }

    #endregion
  }
}