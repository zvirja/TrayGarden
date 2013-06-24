using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;
using TrayGarden.TypesHatcher;
using TrayGarden.UI.WindowWithBackStuff;

namespace TrayGarden.UI
{
    /// <summary>
    /// ViewModel for Window with back
    /// </summary>
    public class WindowWithBackVMBase : INotifyPropertyChanged
    {

        

        protected static bool CommandsEnabledByDefault = true;

        protected RelayCommand _backCommand;
        protected ObservableCollection<ActionCommandVM> _helpActions;
        protected string _copyrightTitle;
        private Stack<WindowWithBackState> _steps;


        protected virtual string BackToTitleInternal { get; set; }

        protected virtual bool CanBack
        {
            get { return _backCommand.CanExecute(null); }
            set { _backCommand.CanExecuteMaster = value; }
        }

        protected virtual Stack<WindowWithBackState> Steps
        {
            get { return _steps; }
            set { _steps = value; }
        }

        protected virtual WindowWithBackState CurrentState
        {
            get { return Steps.Count > 0 ? Steps.Peek() : WindowWithBackState.EmptyState; }
        }


        public WindowWithBackVMBase()
        {
            _backCommand = new RelayCommand(BackExecute, false);
            _helpActions = new ObservableCollection<ActionCommandVM>();
            _helpActions.CollectionChanged += HelpActions_CollectionChanged;
            _copyrightTitle = "Zvirja Inc (c)";
            _steps = new Stack<WindowWithBackState>();
            //_steps.Push(new WindowWithBackState("Tray garden", "Welcome!", "Home", null, null, null));
            _steps.Push(new WindowWithBackState("Tray garden", "Welcome!", "Home",null,new ActionCommandVM(new RelayCommand(ExtraActionExecute,true), "Extra action"), null));
        }



        public event PropertyChangedEventHandler PropertyChanged;

        public virtual string GlobalTitle
        {
            get { return CurrentState.GlobalTitle; }
        }

        public virtual string Header
        {
            get { return CurrentState.Header; }
        }

        public virtual object ContentVM
        {
            get { return CurrentState.ContentVM; }
            set
            {
                if (CurrentState.ContentVM == value) return;
                CurrentState.ContentVM = value;
                OnPropertyChanged("ContentVM");
            }
        }

        [UsedImplicitly]
        public virtual string CopyrightTitle
        {
            get { return _copyrightTitle; }
            set
            {
                if (value == _copyrightTitle) return;
                _copyrightTitle = value;
                OnPropertyChanged("CopyrightTitle");
            }
        }

        //--

        public virtual string BackToTitle
        {
            get { return "Back to " + BackToTitleInternal; }
            set
            {
                if (value == BackToTitleInternal) return;
                BackToTitleInternal = value;
                OnPropertyChanged("BackToTitle");
            }
        }


        [UsedImplicitly]
        public virtual RelayCommand BackCommand
        {
            get { return _backCommand; }
        }

        //--


        [UsedImplicitly]
        public virtual string ExtraActionTitle
        {
            get { return CurrentState.SuperAction.Title; }
        }

        [UsedImplicitly]
        public virtual ICommand ExtraActionCommand
        {
            get { return CurrentState.SuperAction.Command; }
        }

        //---
        /// <summary>
        /// For binding. Returns aggregated collection. Don't use it to assign actions.
        /// </summary>
        public IEnumerable<ActionCommandVM> HelpActions
        {
            get
            {
                var aggregated = new List<ActionCommandVM>(_helpActions);
                Assert.IsNotNull(CurrentState.StateSpecificHelpActions, "StateSpecificHelpActions can't be null");
                aggregated.AddRange(CurrentState.StateSpecificHelpActions);
                return aggregated;
            }
        }

        /// <summary>
        /// Get actions, which are related to whole VM, not to current state.
        /// </summary>
        public ObservableCollection<ActionCommandVM> SelfHelpActions
        {
            get { return _helpActions; }
        }
            
            [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }


        public virtual void ReplaceInitialState(WindowWithBackState newHomeState)
        {
            Steps.Clear();
            Steps.Push(newHomeState);
            CanBack = false;
            NotifyPublicVisibleChanged();
        }


        protected virtual void BackExecute(object o)
        {
            Assert.IsTrue(Steps.Count > 0,"Steps stack is corrupted. Can't be less than 1");
            Steps.Pop();
            CanBack = Steps.Count > 1;
            if (CanBack)
            {
                BackToTitle = Steps.Peek().ShortName;
            }
            NotifyPublicVisibleChanged();
        }



        protected virtual void ExtraActionExecute(object o)
        {
            //TODO REMOVE LATER
            var newStep = new WindowWithBackState("Tray garden - Custom state", "Custom state", "config", null, null,
                                                  null);
            var rc = new RelayCommand(x => MessageBox.Show("Hey"), true);
            newStep.StateSpecificHelpActions.Add(new ActionCommandVM(rc, "Hey Command"));
            newStep.StateSpecificHelpActions.Add(new ActionCommandVM(rc, "Hey Command"));
            GoAheadWithBack(newStep);
        }

        protected virtual void GoAheadWithBack(WindowWithBackState newState)
        {
            BackToTitleInternal = CurrentState.ShortName;
            Steps.Push(newState);
            CanBack = true;
            NotifyPublicVisibleChanged();
        }

        protected virtual void NotifyPublicVisibleChanged()
        {
            OnPropertyChanged("GlobalTitle");
            OnPropertyChanged("Header");
            OnPropertyChanged("ContentVM");
            OnPropertyChanged("BackToTitle");
            OnPropertyChanged("ExtraActionTitle");
            OnPropertyChanged("ExtraActionCommand");
            OnPropertyChanged("HelpActions");
        }


        protected virtual void HelpActions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("HelpActions");
        }

    }
}
