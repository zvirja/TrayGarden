using System;
using TrayGarden.Plants;

namespace TrayGarden.Services.PlantServices.IsEnabledObserver;

internal record IsEnabledObserverPlantBox(IPlantEx PlantEx) : IPlantEnabledInfo
{
    bool IPlantEnabledInfo.IsEnabled => PlantEx.IsEnabled;

    private event EventHandler IsEnabledChanged;

    event EventHandler IPlantEnabledInfo.IsEnabledChanged
    {
        add => this.IsEnabledChanged += value;
        remove => this.IsEnabledChanged -= value;
    }

    public void NotifyIsEnabledChanged()
    {
        IsEnabledChanged?.Invoke(this, EventArgs.Empty);
    }
};