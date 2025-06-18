using System;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Positioning;

public class PositionSize
{
  protected double left;

  protected double mandatoryHeight;

  protected double mandatoryWidth;

  protected double top;

  public PositionSize(double top, double left, double mandatoryWidth, double mandatoryHeight)
  {
    this.top = top;
    this.left = left;
    this.mandatoryWidth = mandatoryWidth;
    this.mandatoryHeight = mandatoryHeight;
  }

  public event Action Changed;

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

  protected virtual void OnChanged()
  {
    Action handler = this.Changed;
    if (handler != null)
    {
      handler();
    }
  }
}