#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

using TrayGarden.UI.Configuration.EntryVM.Players;

#endregion

namespace TrayGarden.UI.Configuration
{
  public interface IConfigurationPlayerWithValues : IConfigurationPlayer
  {
    #region Public Properties

    ICommand Action { get; }

    string ActionTitle { get; }

    bool BoolValue { get; set; }

    double DoubleValue { get; set; }

    int IntValue { get; set; }

    object ObjectValue { get; set; }

    string StringOptionValue { get; set; }

    object StringOptions { get; }

    string StringValue { get; set; }

    #endregion
  }
}