using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.RuntimeSettings
{
  public interface IRuntimeSettingsManager
  {
    ISettingsBox OtherSettings { get; }

    ISettingsBox SystemSettings { get; }

    bool SaveNow(bool force);
  }
}