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
    HeaderText = headerText;
    ButtonText = buttonText;
    LayoutType = ImageTextOrder.VerticalImageText;
    var headerTextBrush = new SolidColorBrush(Color.FromRgb(100, 100, 100));
    headerTextBrush.Freeze();
    HeaderTextDisplayStyle = new TextDisplayOptions(headerTextBrush, 12);
    HeaderTextDisplayStyle.HorizontalAlignment = HorizontalAlignment.Left;
    ButtonTextDisplayStyle = new TextDisplayOptions(Brushes.DarkSlateGray, 20);
    ButtonImageDisplayOptions = new ImageDisplayOptions(64, 64);

    SubmitCommand = new RelayCommand(OnSubmit, true);
  }

  public ImageSource ButtonImage
  {
    get
    {
      return buttonImage;
    }
    set
    {
      buttonImage = value;
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