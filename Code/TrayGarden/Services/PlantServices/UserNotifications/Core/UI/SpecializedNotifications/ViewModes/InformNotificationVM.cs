#region

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

#endregion

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications.ViewModes
{
  public class InformNotificationVM : SpecializedNotificationVMBase, IInformNotification
  {
    #region Constructors and Destructors

    public InformNotificationVM(string textToDisplay)
    {
      this.TextToDisplay = textToDisplay;
      this.OnTextClick = new RelayCommand(this.OnTextClicked, true);
      this.TextDisplayFont = new TextDisplayOptions(Brushes.DimGray, 20.0) { Margins = new Thickness(7) };
    }

    #endregion

    #region Public Properties

    public ICommand OnTextClick { get; protected set; }

    public TextDisplayOptions TextDisplayFont { get; set; }

    public string TextToDisplay { get; set; }

    #endregion

    #region Methods

    protected virtual void OnTextClicked(object obj)
    {
      this.SetResultNotifyInterestedMen(new NotificationResult(ResultCode.OK));
    }

    #endregion
  }
}