using System.Collections.Generic;
using ClipboardChangerPlant.Clipboard;
using ClipboardChangerPlant.Engine;
using ClipboardChangerPlant.NotificationIcon;
using ClipboardChangerPlant.UIConfiguration;

using TrayGarden.Reception;

namespace ClipboardChangerPlant;

public class ClipboardChanger : IPlant, IServicesDelegation
{
  public ClipboardChanger()
  {
    HumanSupportingName = "Clipboard changer";
    Description =
      "This plant listen clipboard and replaces links. \r\nFor instance it shorts all http://www.* like links and resolve direct links for Clip2Net service.";
  }

  public static ClipboardChanger ActualPlant { get; protected set; }

  public string Description { get; protected set; }

  public string HumanSupportingName { get; protected set; }

  public virtual List<object> GetServiceDelegates()
  {
    var result = new List<object>();
    result.Add(NotifyIconManager.ActualManager);
    result.Add(ClipboardManager.Provider);
    result.Add(UIConfigurationManager.ActualManager);
    result.Add(PopupDialogsManager.ActualManager);
    result.Add(GlobalMenuExtender.ActualExtender);
    return result;
  }

  public virtual void Initialize()
  {
    ActualPlant = this;
    AppEngine.ActualEngine.PreInit();
  }

  public virtual void PostServicesInitialize()
  {
    AppEngine.ActualEngine.PostInit();
  }
}