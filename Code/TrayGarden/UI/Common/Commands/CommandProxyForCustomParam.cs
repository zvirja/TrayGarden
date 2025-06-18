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
    InternalCommand = command;
    ParamToPass = argumentToPass;
    InternalCommand.CanExecuteChanged += InternalCommand_CanExecuteChanged;
  }

  public event EventHandler CanExecuteChanged;

  public ICommand InternalCommand { get; set; }

  public object ParamToPass { get; set; }

  public bool CanExecute(object parameter)
  {
    return InternalCommand.CanExecute(parameter);
  }

  public void Execute(object parameter)
  {
    InternalCommand.Execute(ParamToPass);
  }

  private void InternalCommand_CanExecuteChanged(object sender, EventArgs e)
  {
    if (CanExecuteChanged == null)
    {
      return;
    }
    CanExecuteChanged(sender, e);
  }
}