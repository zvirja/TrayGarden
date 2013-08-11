using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace TrayGarden.UI.Common.Commands
{
    public class ActionCommandVM
    {
       
        public ICommand Command { get; protected set; }
        public string Title { get; protected set; }

        public ActionCommandVM(ICommand command, string title)
        {
            Command = command;
            Title = title;
        }


    }
}
