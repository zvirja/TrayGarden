using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

using TrayGarden.UI.Configuration.EntryVM;

namespace TrayGarden.UI.Configuration
{
  public interface IConfigurationControlVM
  {
    List<ConfigurationEntryBaseVM> ConfigurationEntries { get; }

    ICommand ResetAll { get; }
  }
}