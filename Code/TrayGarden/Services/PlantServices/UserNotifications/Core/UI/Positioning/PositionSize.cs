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
      return left;
    }
    set
    {
      left = value;
      OnChanged();
    }
  }

  public double MandatoryHeight
  {
    get
    {
      return mandatoryHeight;
    }
    set
    {
      mandatoryHeight = value;
      OnChanged();
    }
  }

  public double MandatoryWidth
  {
    get
    {
      return mandatoryWidth;
    }
    set
    {
      mandatoryWidth = value;
      OnChanged();
    }
  }

  public double Top
  {
    get
    {
      return top;
    }
    set
    {
      if (Math.Abs(top - value) < 0.1)
      {
        return;
      }
      top = value;
      OnChanged();
    }
  }

  protected virtual void OnChanged()
  {
    Action handler = Changed;
    if (handler != null)
    {
      handler();
    }
  }
}