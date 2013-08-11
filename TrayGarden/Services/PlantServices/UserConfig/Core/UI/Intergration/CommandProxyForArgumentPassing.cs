using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.UI.Intergration
{
    public class CommandProxyForArgumentPassing: ICommand
    {
        public object ParamToPass { get; set; }
        public ICommand InternalCommand { get; set; }

        public CommandProxyForArgumentPassing([NotNull] ICommand command, [NotNull] object argumentToPass)
        {
            Assert.ArgumentNotNull(command, "command");
            Assert.ArgumentNotNull(argumentToPass, "argumentToPass");
            InternalCommand = command;
            ParamToPass = argumentToPass;
            InternalCommand.CanExecuteChanged += InternalCommand_CanExecuteChanged;
        }

        void InternalCommand_CanExecuteChanged(object sender, EventArgs e)
        {
            if (CanExecuteChanged == null) return;
            CanExecuteChanged(sender, e);
        }

        public void Execute(object parameter)
        {
            InternalCommand.Execute(ParamToPass);
        }

        public bool CanExecute(object parameter)
        {
            return InternalCommand.CanExecute(parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}
