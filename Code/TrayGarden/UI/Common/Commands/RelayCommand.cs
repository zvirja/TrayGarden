using System;
using System.Diagnostics;
using System.Windows.Input;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;

namespace TrayGarden.UI.Common.Commands;

public class RelayCommand : ICommand
{
  protected readonly Predicate<object> _canExecute;

  protected readonly Action<object> _execute;

  protected bool _canExecuteMaster;

  public RelayCommand([NotNull] Action<object> execute, bool canExecute = true)
  {
    Assert.ArgumentNotNull(execute, "execute");
    _canExecuteMaster = canExecute;
    _execute = execute;
  }

  public RelayCommand(Action<object> execute, Predicate<object> canExecute)
  {
    Assert.ArgumentNotNull(execute, "execute");
    _execute = execute;
    _canExecute = canExecute;
  }

  public event EventHandler CanExecuteChanged
  {
    add
    {
      CommandManager.RequerySuggested += value;
    }
    remove
    {
      CommandManager.RequerySuggested -= value;
    }
  }

  public bool CanExecuteMaster
  {
    get
    {
      return _canExecuteMaster;
    }
    set
    {
      if (_canExecuteMaster == value)
      {
        return;
      }
      _canExecuteMaster = value;
      CommandManager.InvalidateRequerySuggested();
    }
  }

  [DebuggerStepThrough]
  public virtual bool CanExecute(object parameter)
  {
    return _canExecute != null ? _canExecute(parameter) : CanExecuteMaster;
  }

  public virtual void Execute(object parameter)
  {
    _execute(parameter);
  }
}