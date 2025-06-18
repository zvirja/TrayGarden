using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Services.Engine;

public interface IServicesSteward
{
  List<IService> Services { get; set; }

  void InformClosingStage();

  void InformDisplayStage();

  void InformInitializeStage();
}