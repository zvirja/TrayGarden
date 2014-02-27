#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Helpers;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications.ViewModes;

#endregion

namespace ClipboardChangerPlant.RequestHandling.Specialized
{
  [UsedImplicitly]
  public class UrlShortenerWithConfirmation : RequestHandlerWithUIConfirmation
  {
    #region Properties

    protected string OperableUrlValue { get; set; }

    #endregion

    #region Public Methods and Operators

    public override bool PreExecute(string operableUrl, bool isClipboardRequest)
    {
      this.OperableUrlValue = operableUrl;
      return base.PreExecute(operableUrl, isClipboardRequest);
    }

    #endregion

    #region Methods

    protected override IResultProvider GetConfirmationDialog()
    {
      return new YesNoNotificationVM("Short url?{0}{1}".FormatWith(Environment.NewLine, this.OperableUrlValue));
    }

    protected override IResultProvider GetRevertDialog()
    {
      return new YesNoNotificationVM("Revert shortening?{0}{1}".FormatWith(Environment.NewLine, this.OperableUrlValue));
    }

    #endregion
  }
}