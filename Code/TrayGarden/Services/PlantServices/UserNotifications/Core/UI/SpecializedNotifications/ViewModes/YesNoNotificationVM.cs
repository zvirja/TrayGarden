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
    this.HeaderText = headerText;
    this.HeaderTextOptions = new TextDisplayOptions(Brushes.DimGray, 14);
    this.ButtonsLayoutKind = ImageTextOrder.VerticalImageText;
    this.ButtonImagesDisplayOptions = new ImageDisplayOptions(32, 32);
    this.ButtonTextsOptions = new TextDisplayOptions(Brushes.DarkSlateGray, 20);

    //Yes button init
    this.YesButtonText = "YES";
    this.YesAction = new RelayCommand(this.OnYesAction, true);

    //No button init
    this.NoButtonText = "NO";
    this.NoAction = new RelayCommand(this.OnNoAction, true);
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
    this.SetResultNotifyInterestedMen(new NotificationResult(ResultCode.No));
  }

  protected virtual void OnYesAction(object o)
  {
    this.SetResultNotifyInterestedMen(new NotificationResult(ResultCode.Yes));
  }
}