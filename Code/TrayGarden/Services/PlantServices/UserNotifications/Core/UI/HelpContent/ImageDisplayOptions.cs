using System.Windows;
using System.Windows.Media;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.HelpContent;

public class ImageDisplayOptions
{
  public ImageDisplayOptions(double width, double height)
  {
    this.Width = width;
    this.Height = height;
    this.Stretch = Stretch.Fill;
    this.Margins = new Thickness(0);
    this.HorizontalAlignment = HorizontalAlignment.Center;
    this.VerticalAlignment = VerticalAlignment.Stretch;
  }

  public double Height { get; set; }

  public HorizontalAlignment HorizontalAlignment { get; set; }

  public Thickness Margins { get; set; }

  public Stretch Stretch { get; set; }

  public VerticalAlignment VerticalAlignment { get; set; }

  public double Width { get; set; }
}