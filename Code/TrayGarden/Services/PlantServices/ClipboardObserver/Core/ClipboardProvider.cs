#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Services.PlantServices.ClipboardObserver.Core
{
  public class ClipboardProvider : IClipboardProvider
  {
    #region Constructors and Destructors

    public ClipboardProvider(ClipboardObserverService service)
    {
      this.Service = service;
    }

    #endregion

    #region Properties

    protected ClipboardObserverService Service { get; set; }

    #endregion

    #region Public Methods and Operators

    public virtual string GetCurrentClipboardText()
    {
      return this.Service.GetClipboardValue(false);
    }

    public virtual string GetCurrentClipboardTextIgnoreSizeRestrictions()
    {
      return this.Service.GetClipboardValue(true);
    }

    public virtual void SetCurrentClipboardText(string newValue, bool silent)
    {
      this.Service.SetClipboardValue(newValue, silent);
    }

    #endregion
  }
}