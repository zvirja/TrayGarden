using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TrayGarden.Services.PlantServices.ClipboardObserver.Core
{
  public interface IClipboardProvider
  {
    string GetCurrentClipboardText();
    void SetCurrentClipboardText(string newValue);
  }
}
