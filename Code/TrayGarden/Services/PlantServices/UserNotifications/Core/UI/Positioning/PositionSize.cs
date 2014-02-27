#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Positioning
{
  public class PositionSize
  {
    #region Fields

    protected double left;

    protected double mandatoryHeight;

    protected double mandatoryWidth;

    protected double top;

    #endregion

    #region Constructors and Destructors

    public PositionSize(double top, double left, double mandatoryWidth, double mandatoryHeight)
    {
      this.top = top;
      this.left = left;
      this.mandatoryWidth = mandatoryWidth;
      this.mandatoryHeight = mandatoryHeight;
    }

    #endregion

    #region Public Events

    public event Action Changed;

    #endregion

    #region Public Properties

    public double Left
    {
      get
      {
        return this.left;
      }
      set
      {
        this.left = value;
        this.OnChanged();
      }
    }

    public double MandatoryHeight
    {
      get
      {
        return this.mandatoryHeight;
      }
      set
      {
        this.mandatoryHeight = value;
        this.OnChanged();
      }
    }

    public double MandatoryWidth
    {
      get
      {
        return this.mandatoryWidth;
      }
      set
      {
        this.mandatoryWidth = value;
        this.OnChanged();
      }
    }

    public double Top
    {
      get
      {
        return this.top;
      }
      set
      {
        if (Math.Abs(this.top - value) < 0.1)
        {
          return;
        }
        this.top = value;
        this.OnChanged();
      }
    }

    #endregion

    #region Methods

    protected virtual void OnChanged()
    {
      Action handler = this.Changed;
      if (handler != null)
      {
        handler();
      }
    }

    #endregion
  }
}