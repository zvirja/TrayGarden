using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

namespace TrayGarden.UI.Configuration.EntryVM.ExtentedEntry;

public interface IConfigurationEntryAction
{
  ICommand Action { get; }

  string Hint { get; }

  ImageSource LabelImage { get; }
}