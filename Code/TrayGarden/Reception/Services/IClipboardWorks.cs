using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Services.PlantServices.ClipboardObserver.Core;

namespace TrayGarden.Reception.Services
{
  /// <summary>
  /// This service allows to get the clipboard provider. 
  /// Service is enabled in any case
  /// </summary>
  public interface IClipboardWorks
  {
    void StoreClipboardValueProvider(IClipboardProvider provider);
  }
}