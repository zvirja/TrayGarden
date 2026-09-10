using System.Collections.Generic;
using TrayGarden.UI.Common.Commands;
using TrayGarden.UI.WindowWithReturn;

namespace TrayGarden.UI.ForSimplerLife;

public class WindowWithBackStateConstructInfo
{
  public object ContentVM { get; set; }

  public string GlobalTitle { get; set; }

  public string Header { get; set; }

  public WindowStepState ResultState { get; set; }

  public string ShortName { get; set; }

  public List<ActionCommandVM> StateSpecificHelpActions { get; set; }

  public ActionCommandVM SuperAction { get; set; }
}