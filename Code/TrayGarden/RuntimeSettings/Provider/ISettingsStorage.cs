using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.RuntimeSettings.Provider;

public interface ISettingsStorage
{
  IContainer GetRootContainer();

  void LoadSettings();

  bool SaveSettings();
}