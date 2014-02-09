#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;

#endregion

namespace TrayGarden.UI.Common.Commands
{
  /// <summary>
  /// This class is a wrapper on top of command. It substitutes custom object, which is passed to the Execute.
  /// </summary>
  public class CommandProxyForCustomParam : ICommand
  {
    #region Constructors and Destructors

    public CommandProxyForCustomParam([NotNull] ICommand command, [NotNull] object argumentToPass)
    {
      Assert.ArgumentNotNull(command, "command");
      Assert.ArgumentNotNull(argumentToPass, "argumentToPass");
      this.InternalCommand = command;
      this.ParamToPass = argumentToPass;
      this.InternalCommand.CanExecuteChanged += this.InternalCommand_CanExecuteChanged;
    }

    #endregion

    #region Public Events

    public event EventHandler CanExecuteChanged;

    #endregion

    #region Public Properties

    public ICommand InternalCommand { get; set; }

    public object ParamToPass { get; set; }

    #endregion

    #region Public Methods and Operators

    public bool CanExecute(object parameter)
    {
      return this.InternalCommand.CanExecute(parameter);
    }

    public void Execute(object parameter)
    {
      this.InternalCommand.Execute(this.ParamToPass);
    }

    #endregion

    #region Methods

    private void InternalCommand_CanExecuteChanged(object sender, EventArgs e)
    {
      if (this.CanExecuteChanged == null)
      {
        return;
      }
      this.CanExecuteChanged(sender, e);
    }

    #endregion
  }
}