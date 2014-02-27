#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

#endregion

namespace TrayGarden.UI.Common.Commands
{
  public class ActionCommandVM
  {
    #region Constructors and Destructors

    public ActionCommandVM(ICommand command, string title)
    {
      this.Command = command;
      this.Title = title;
    }

    #endregion

    #region Public Properties

    public ICommand Command { get; protected set; }

    public string Title { get; protected set; }

    #endregion
  }
}