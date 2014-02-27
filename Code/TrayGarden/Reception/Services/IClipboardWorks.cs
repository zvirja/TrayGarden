#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Services.PlantServices.ClipboardObserver.Core;

#endregion

namespace TrayGarden.Reception.Services
{
  /// <summary>
  /// This service allows to get the clipboard provider. 
  /// Service is enabled in any case
  /// </summary>
  public interface IClipboardWorks
  {
    #region Public Methods and Operators

    void StoreClipboardValueProvider(IClipboardProvider provider);

    #endregion
  }
}