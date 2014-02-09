#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;

#endregion

namespace TrayGarden.UI.Common.Commands
{
  public class RelayCommand : ICommand
  {
    #region Fields

    protected readonly Predicate<object> _canExecute;

    protected readonly Action<object> _execute;

    protected bool _canExecuteMaster;

    #endregion

    #region Constructors and Destructors

    public RelayCommand([NotNull] Action<object> execute, bool canExecute = true)
    {
      Assert.ArgumentNotNull(execute, "execute");
      this._canExecuteMaster = canExecute;
      this._execute = execute;
    }

    public RelayCommand(Action<object> execute, Predicate<object> canExecute)
    {
      Assert.ArgumentNotNull(execute, "execute");
      this._execute = execute;
      this._canExecute = canExecute;
    }

    #endregion

    #region Public Events

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

    #endregion

    #region Public Properties

    public bool CanExecuteMaster
    {
      get
      {
        return this._canExecuteMaster;
      }
      set
      {
        if (this._canExecuteMaster == value)
        {
          return;
        }
        this._canExecuteMaster = value;
        CommandManager.InvalidateRequerySuggested();
      }
    }

    #endregion

    #region Public Methods and Operators

    [DebuggerStepThrough]
    public virtual bool CanExecute(object parameter)
    {
      return this._canExecute != null ? this._canExecute(parameter) : this.CanExecuteMaster;
    }

    public virtual void Execute(object parameter)
    {
      this._execute(parameter);
    }

    #endregion
  }
}