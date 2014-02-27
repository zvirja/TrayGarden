#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Plants;

#endregion

namespace TrayGarden.Services
{
  public interface IService
  {
    #region Public Events

    event Action<bool> IsEnabledChanged;

    #endregion

    #region Public Properties

    bool CanBeDisabled { get; }

    bool IsActuallyEnabled { get; }

    bool IsEnabled { get; set; }

    string LuggageName { get; }

    string ServiceDescription { get; }

    string ServiceName { get; }

    #endregion

    #region Public Methods and Operators

    void InformClosingStage();

    void InformDisplayStage();

    void InformInitializeStage();

    void InitializePlant(IPlantEx plantEx);

    bool IsAvailableForPlant(IPlantEx plantEx);

    #endregion
  }
}