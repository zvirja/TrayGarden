using System;
using System.Windows.Input;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;

namespace TrayGarden.UI.Common.Commands;

/// <summary>
/// This class is a wrapper on top of command. It substitutes custom object, which is passed to the Execute.
/// </summary>
public class CommandProxyForCustomParam : ICommand
{
  public CommandProxyForCustomParam([NotNull] ICommand command, [NotNull] object argumentToPass)
  {
    Assert.ArgumentNotNull(command, "command");
    Assert.ArgumentNotNull(argumentToPass, "argumentToPass");
    this.InternalCommand = command;
    this.ParamToPass = argumentToPass;
    this.InternalCommand.CanExecuteChanged += this.InternalCommand_CanExecuteChanged;
  }

  public event EventHandler CanExecuteChanged;

  public ICommand InternalCommand { get; set; }

  public object ParamToPass { get; set; }

  public bool CanExecute(object parameter)
  {
    return this.InternalCommand.CanExecute(parameter);
  }

  public void Execute(object parameter)
  {
    this.InternalCommand.Execute(this.ParamToPass);
  }

  private void InternalCommand_CanExecuteChanged(object sender, EventArgs e)
  {
    if (this.CanExecuteChanged == null)
    {
      return;
    }
    this.CanExecuteChanged(sender, e);
  }
}