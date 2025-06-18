using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.HelpContent;

public class TextDisplayOptions
{
  private Brush brush;

  public TextDisplayOptions(Brush brush, double size)
  {
    this.ValidateBrushWithException(brush);
    this.Brush = brush;
    this.Size = size;
    this.Weight = FontWeights.Normal;
    this.Style = FontStyles.Normal;
    this.Margins = new Thickness();
    this.HorizontalAlignment = HorizontalAlignment.Center;
    this.VerticalAlignment = VerticalAlignment.Center;
    this.TextAlignment = TextAlignment.Center;
    this.Wrapping = TextWrapping.Wrap;
  }

  public Brush Brush
  {
    get
    {
      return this.brush;
    }
    set
    {
      this.ValidateBrushWithException(this.brush);
      this.brush = value;
    }
  }

  public HorizontalAlignment HorizontalAlignment { get; set; }

  public Thickness Margins { get; set; }

  public double Size { get; set; }

  public FontStyle Style { get; set; }

  public TextAlignment TextAlignment { get; set; }

  public VerticalAlignment VerticalAlignment { get; set; }

  public FontWeight Weight { get; set; }

  public TextWrapping Wrapping { get; set; }

  protected void ValidateBrushWithException(Brush brush)
  {
    if (brush != null && !brush.IsFrozen)
    {
      throw new ArgumentException("Brush have to be frozen!");
    }
  }
}