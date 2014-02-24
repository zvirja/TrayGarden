using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.HelpContent
{
  public class ImageDisplayOptions
  {
    public double Width { get; set; }
    public double Height { get; set; }
    public Stretch Stretch { get; set; }
    public Thickness Margins { get; set; }
    public HorizontalAlignment HorizontalAlignment { get; set; }
    public VerticalAlignment VerticalAlignment { get; set; }

    public ImageDisplayOptions(double width, double height)
    {
      Width = width;
      Height = height;
      Stretch = Stretch.Fill;
      Margins = new Thickness(0);
      HorizontalAlignment = HorizontalAlignment.Center;
      VerticalAlignment = VerticalAlignment.Stretch;
    }
  }
}
