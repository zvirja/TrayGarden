using System.Collections.Generic;

namespace TrayGarden.Services.Engine;

public interface IServicesSteward
{
  List<IService> Services { get; set; }

  void InformClosingStage();

  void InformDisplayStage();

  void InformInitializeStage();
}