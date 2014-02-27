#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ClipboardChangerPlant.Clipboard;
using ClipboardChangerPlant.Engine;
using ClipboardChangerPlant.NotificationIcon;
using ClipboardChangerPlant.UIConfiguration;

using TrayGarden.Reception;

#endregion

namespace ClipboardChangerPlant
{
  public class ClipboardChanger : IPlant, IServicesDelegation
  {
    #region Constructors and Destructors

    public ClipboardChanger()
    {
      this.HumanSupportingName = "Clipboard changer";
      this.Description =
        "This plant listen clipboard and replaces links. \r\nFor instance it shorts all http://www.* like links and resolve direct links for Clip2Net service.";
    }

    #endregion

    #region Public Properties

    public static ClipboardChanger ActualPlant { get; protected set; }

    public string Description { get; protected set; }

    public string HumanSupportingName { get; protected set; }

    #endregion

    #region Public Methods and Operators

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

    #endregion
  }
}