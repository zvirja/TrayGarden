using System.Windows.Input;
using System.Windows.Media;

using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.HelpContent;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications.Interfaces;
using TrayGarden.UI.Common.Commands;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications.ViewModes;

public class YesNoNotificationVM : SpecializedNotificationVMBase, IYesNoNotification
{
  public YesNoNotificationVM(string headerText)
  {
    //Common init
    HeaderText = headerText;
    HeaderTextOptions = new TextDisplayOptions(Brushes.DimGray, 14);
    ButtonsLayoutKind = ImageTextOrder.VerticalImageText;
    ButtonImagesDisplayOptions = new ImageDisplayOptions(32, 32);
    ButtonTextsOptions = new TextDisplayOptions(Brushes.DarkSlateGray, 20);

    //Yes button init
    YesButtonText = "YES";
    YesAction = new RelayCommand(OnYesAction, true);

    //No button init
    NoButtonText = "NO";
    NoAction = new RelayCommand(OnNoAction, true);
  }

  public ImageDisplayOptions ButtonImagesDisplayOptions { get; set; }

  public TextDisplayOptions ButtonTextsOptions { get; set; }

  public ImageTextOrder ButtonsLayoutKind { get; set; }

  public string HeaderText { get; set; }

  public TextDisplayOptions HeaderTextOptions { get; set; }

  public ICommand NoAction { get; set; }

  public ImageSource NoButtonImage { get; set; }

  public string NoButtonText { get; set; }

  public ICommand YesAction { get; set; }

  public ImageSource YesButtonImage { get; set; }

  public string YesButtonText { get; set; }

  protected virtual void OnNoAction(object o)
  {
    SetResultNotifyInterestedMen(new NotificationResult(ResultCode.No));
  }

  protected virtual void OnYesAction(object o)
  {
    SetResultNotifyInterestedMen(new NotificationResult(ResultCode.Yes));
  }
}