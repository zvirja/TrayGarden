using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.HelpContent;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications.Interfaces;
using TrayGarden.UI.Common.Commands;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications.ViewModes;

public class ActionNotificationVM : SpecializedNotificationVMBase, IActionNotification
{
  private ImageSource buttonImage;

  public ActionNotificationVM(string headerText, string buttonText)
  {
    this.HeaderText = headerText;
    this.ButtonText = buttonText;
    this.LayoutType = ImageTextOrder.VerticalImageText;
    var headerTextBrush = new SolidColorBrush(Color.FromRgb(100, 100, 100));
    headerTextBrush.Freeze();
    this.HeaderTextDisplayStyle = new TextDisplayOptions(headerTextBrush, 12);
    this.HeaderTextDisplayStyle.HorizontalAlignment = HorizontalAlignment.Left;
    this.ButtonTextDisplayStyle = new TextDisplayOptions(Brushes.DarkSlateGray, 20);
    this.ButtonImageDisplayOptions = new ImageDisplayOptions(64, 64);

    this.SubmitCommand = new RelayCommand(this.OnSubmit, true);
  }

  public ImageSource ButtonImage
  {
    get
    {
      return this.buttonImage;
    }
    set
    {
      this.buttonImage = value;
    }
  }

  public ImageDisplayOptions ButtonImageDisplayOptions { get; set; }

  public string ButtonText { get; set; }

  public TextDisplayOptions ButtonTextDisplayStyle { get; set; }

  public string HeaderText { get; set; }

  public TextDisplayOptions HeaderTextDisplayStyle { get; set; }

  public ImageTextOrder LayoutType { get; set; }

  public ICommand SubmitCommand { get; set; }

  protected virtual void OnSubmit(object o)
  {
    base.SetResultNotifyInterestedMen(new NotificationResult(ResultCode.OK));
  }
}