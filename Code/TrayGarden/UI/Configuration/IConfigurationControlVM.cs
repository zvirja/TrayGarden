#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

using TrayGarden.UI.Configuration.EntryVM;

#endregion

namespace TrayGarden.UI.Configuration
{
  public interface IConfigurationControlVM
  {
    #region Public Properties

    List<ConfigurationEntryBaseVM> ConfigurationEntries { get; }

    ICommand ResetAll { get; }

    #endregion
  }
}