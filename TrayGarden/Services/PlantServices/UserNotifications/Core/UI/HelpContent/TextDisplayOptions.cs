using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.HelpContent
{
  public class TextDisplayOptions
  {
    private Brush brush;
    public Brush Brush
    {
      get { return brush; }
      set
      {
        ValidateBrushWithException(brush);
        brush = value;
      }
    }

    public FontWeight Weight { get; set; }
    public FontStyle Style { get; set; }
    public double Size { get; set; }
    public Thickness Margins { get; set; }
    public HorizontalAlignment HorizontalAlignment { get; set; }
    public VerticalAlignment VerticalAlignment { get; set; }
    public TextAlignment TextAlignment { get; set; }
    public TextWrapping Wrapping { get; set; }

    public TextDisplayOptions(Brush brush, double size)
    {
      ValidateBrushWithException(brush);
      Brush = brush;
      Size = size;
      Weight = FontWeights.Normal;
      Style = FontStyles.Normal;
      Margins = new Thickness();
      HorizontalAlignment = HorizontalAlignment.Center;
      VerticalAlignment = VerticalAlignment.Center;
      TextAlignment = TextAlignment.Center;
      Wrapping = TextWrapping.Wrap;
    }

    protected void ValidateBrushWithException(Brush brush)
    {
      if (brush != null && !brush.IsFrozen)
        throw new ArgumentException("Brush have to be frozen!");
    }

    
  }
}
