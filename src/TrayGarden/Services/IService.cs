using System;
using TrayGarden.Plants;

namespace TrayGarden.Services;

public interface IService
{
  event Action<bool> IsEnabledChanged;

  bool CanBeDisabled { get; }

  bool IsActuallyEnabled { get; }

  bool IsEnabled { get; set; }

  string LuggageName { get; }

  string ServiceDescription { get; }

  string ServiceName { get; }

  void InformClosingStage();

  void InformDisplayStage();

  void InformInitializeStage();

  void InitializePlant(IPlantEx plantEx);

  bool IsAvailableForPlant(IPlantEx plantEx);
}