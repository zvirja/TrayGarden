using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.HelpContent;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;
using TrayGarden.UI.Common.Commands;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using FontStyle = System.Windows.FontStyle;
using Image = System.Windows.Controls.Image;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications.ViewModes
{
  public class InformNotificationVM:SpecializedNotificationVMBase
  {
    public string TextToDisplay { get; protected set; }
    public ICommand OnTextClick { get; protected set; }

    public TextDisplayOptions TextDisplayFont { get; set; }

    public InformNotificationVM(string textToDisplay):  this(textToDisplay,20.0)
    {
    }

    public InformNotificationVM(string textToDisplay, double textSize)
    {
      TextToDisplay = textToDisplay;
      OnTextClick = new RelayCommand(OnTextClicked, true);
      TextDisplayFont = new TextDisplayOptions(Brushes.DimGray, 20.0){Margins = new Thickness(7)};
    }

    protected virtual void OnTextClicked(object obj)
    {
      SetResultNotifyInterestedMen(new NotificationResult(ResultCode.OK));
    }
  }
}
