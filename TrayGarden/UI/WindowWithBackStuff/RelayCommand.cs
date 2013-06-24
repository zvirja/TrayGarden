using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;

namespace TrayGarden.UI.WindowWithBackStuff
{
    public class RelayCommand : ICommand
    {

        #region Fields 

        protected readonly Action<object> _execute;
        protected readonly Predicate<object> _canExecute;
        protected bool _canExecuteMaster;

        public bool CanExecuteMaster
        {
            get { return _canExecuteMaster; }
            set
            {
                if (_canExecuteMaster == value) return;
                _canExecuteMaster = value;
                CommandManager.InvalidateRequerySuggested();
            }
        }

        #endregion // Fields 

        #region Constructors

        public RelayCommand(Action<object> execute) : this(execute, true)
        {
        }

        public RelayCommand([NotNull] Action<object> execute, bool canExecute)
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

        #endregion // Constructors 

        #region ICommand Members 

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute != null ? _canExecute(parameter) : CanExecuteMaster;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        #endregion // ICommand Members
    }
}
