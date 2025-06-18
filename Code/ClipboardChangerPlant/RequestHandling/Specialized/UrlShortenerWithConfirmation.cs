using System;
using JetBrains.Annotations;

using TrayGarden.Helpers;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications.ViewModes;

namespace ClipboardChangerPlant.RequestHandling.Specialized;

[UsedImplicitly]
public class UrlShortenerWithConfirmation : RequestHandlerWithUIConfirmation
{
  protected string OperableUrlValue { get; set; }

  public override bool PreExecute(string operableUrl, bool isClipboardRequest)
  {
    this.OperableUrlValue = operableUrl;
    return base.PreExecute(operableUrl, isClipboardRequest);
  }

  protected override IResultProvider GetConfirmationDialog()
  {
    return new YesNoNotificationVM("Short url?{0}{1}".FormatWith(Environment.NewLine, this.OperableUrlValue));
  }

  protected override IResultProvider GetRevertDialog()
  {
    return new YesNoNotificationVM("Revert shortening?{0}{1}".FormatWith(Environment.NewLine, this.OperableUrlValue));
  }
}