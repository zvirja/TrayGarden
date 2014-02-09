#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

#endregion

namespace TrayGarden.UI.Configuration.EntryVM.Players
{
  public interface IActionConfigurationPlayer : IConfigurationPlayer
  {
    #region Public Properties

    ICommand Action { get; }

    string ActionTitle { get; }

    #endregion
  }
}