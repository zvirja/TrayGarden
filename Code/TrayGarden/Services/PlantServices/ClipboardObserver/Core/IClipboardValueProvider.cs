#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Services.PlantServices.ClipboardObserver.Core
{
  public interface IClipboardProvider
  {
    #region Public Methods and Operators

    string GetCurrentClipboardText();

    string GetCurrentClipboardTextIgnoreSizeRestrictions();

    void SetCurrentClipboardText(string newValue, bool silent);

    #endregion
  }
}