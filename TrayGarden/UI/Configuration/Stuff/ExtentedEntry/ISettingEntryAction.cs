using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using JetBrains.Annotations;

namespace TrayGarden.UI.Configuration.Stuff.ExtentedEntry
{
  public interface ISettingEntryAction
  {
    string Hint { get; }
    ICommand Action { get; }
    ImageSource LabelImage { get; }
  }
}
