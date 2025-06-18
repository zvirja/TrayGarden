using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace TrayGarden.UI.Configuration.EntryVM.Players;

public interface IActionConfigurationPlayer : IConfigurationPlayer
{
  ICommand Action { get; }

  string ActionTitle { get; }
}