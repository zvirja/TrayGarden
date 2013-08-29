using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClipboardChangerPlant.Clipboard;
using ClipboardChangerPlant.Engine;
using ClipboardChangerPlant.NotificationIcon;
using ClipboardChangerPlant.UIConfiguration;
using TrayGarden.Reception;

namespace ClipboardChangerPlant
{
  public class ClipboardChanger:IPlant, IServicesDelegation
  {
    public static ClipboardChanger ActualPlant { get; protected set; }
    
    public string HumanSupportingName { get; protected set; }
    public string Description { get; protected set; }

    public ClipboardChanger()
    {
      HumanSupportingName = "Clipboard changer";
      Description =
        "This plant listen clipboard and replaces links. \r\nFor instance it shorts all http://www.* like links and resolve direct links for Clip2Net service.";
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

    public virtual List<object> GetServiceDelegates()
    {
      var result = new List<object>();
      result.Add(NotifyIconManager.ActualManager);
      result.Add(ClipboardManager.Provider);
      result.Add(UIConfigurationManager.ActualManager);
      return result;
    }
  }
}
