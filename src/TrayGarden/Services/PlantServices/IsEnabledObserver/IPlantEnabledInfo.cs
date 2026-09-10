using System;

namespace TrayGarden.Services.PlantServices.IsEnabledObserver;

public interface IPlantEnabledInfo
{
    bool IsEnabled { get; }
    
    event EventHandler IsEnabledChanged;
}