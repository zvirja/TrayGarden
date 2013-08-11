using System.Collections.Generic;
using System.Windows.Input;
using TrayGarden.UI.Configuration.Stuff;

namespace TrayGarden.UI.Configuration
{
    public interface IConfigurationControlVM
    {
        List<ConfigurationEntryVMBase> ConfigurationEntries { get; }
        ICommand ResetAll { get; }
    }
}