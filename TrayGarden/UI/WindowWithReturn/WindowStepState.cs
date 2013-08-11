using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;
using TrayGarden.UI.Common.Commands;

namespace TrayGarden.UI.WindowWithReturn
{
    public class WindowStepState
    {

        public static WindowStepState EmptyState;

        static WindowStepState()
        {
            EmptyState = new WindowStepState();
            EmptyState.SuperAction = new ActionCommandVM(new RelayCommand(delegate { throw new InvalidOperationException("Command can't be executed!"); }, false), string.Empty);
            EmptyState.StateSpecificHelpActions = new List<ActionCommandVM>();
            EmptyState.ContentVM = DependencyProperty.UnsetValue;
        }

        public string GlobalTitle { get;  set; }
        public string Header { get;  set; }
        public string ShortName { get;  set; }
        public object ContentVM { get;  set; }
        public ActionCommandVM SuperAction { get;  set; }
        public List<ActionCommandVM> StateSpecificHelpActions { get; set; }

        public WindowStepState([NotNull] string globalTitle, [NotNull] string header, [NotNull] string shortName, object contentVM,
                                   ActionCommandVM superAction, List<ActionCommandVM> stateSpecificHelpActions)
        {
            Assert.ArgumentNotNullOrEmpty(shortName, "shortName");
            Assert.ArgumentNotNullOrEmpty(globalTitle, "globalTitle");
            Assert.ArgumentNotNullOrEmpty(header, "header");
            GlobalTitle = globalTitle;
            Header = header;
            ShortName = shortName;
            SuperAction = superAction ?? EmptyState.SuperAction;
            ContentVM = contentVM ?? DependencyProperty.UnsetValue;
            StateSpecificHelpActions = stateSpecificHelpActions ?? new List<ActionCommandVM>();
        }

        protected WindowStepState()
        {
        }
    }
}
