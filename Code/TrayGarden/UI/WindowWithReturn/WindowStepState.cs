#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.UI.Common.Commands;

#endregion

namespace TrayGarden.UI.WindowWithReturn
{
  public class WindowStepState
  {
    #region Static Fields

    public static WindowStepState EmptyState;

    #endregion

    #region Constructors and Destructors

    static WindowStepState()
    {
      EmptyState = new WindowStepState();
      EmptyState.SuperAction =
        new ActionCommandVM(
          new RelayCommand(delegate { throw new InvalidOperationException("Command can't be executed!"); }, false),
          string.Empty);
      EmptyState.StateSpecificHelpActions = new List<ActionCommandVM>();
      EmptyState.ContentVM = DependencyProperty.UnsetValue;
    }

    public WindowStepState(
      [NotNull] string globalTitle,
      [NotNull] string header,
      [NotNull] string shortName,
      object contentVM,
      ActionCommandVM superAction,
      List<ActionCommandVM> stateSpecificHelpActions)
    {
      Assert.ArgumentNotNullOrEmpty(shortName, "shortName");
      Assert.ArgumentNotNullOrEmpty(globalTitle, "globalTitle");
      Assert.ArgumentNotNullOrEmpty(header, "header");
      this.GlobalTitle = globalTitle;
      this.Header = header;
      this.ShortName = shortName;
      this.SuperAction = superAction ?? EmptyState.SuperAction;
      this.ContentVM = contentVM ?? DependencyProperty.UnsetValue;
      this.StateSpecificHelpActions = stateSpecificHelpActions ?? new List<ActionCommandVM>();
    }

    protected WindowStepState()
    {
    }

    #endregion

    #region Public Properties

    public object ContentVM { get; set; }

    public string GlobalTitle { get; set; }

    public string Header { get; set; }

    public string ShortName { get; set; }

    public List<ActionCommandVM> StateSpecificHelpActions { get; set; }

    public ActionCommandVM SuperAction { get; set; }

    #endregion
  }
}