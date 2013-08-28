using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrayGarden.Configuration;
using TrayGarden.Reception;
using TrayGarden.Reception.Services;
using TrayGarden.Reception.Services.StandaloneIcon;
using TrayGarden.Resources;
using TrayGarden.RuntimeSettings;
using TrayGarden.Services.FleaMarket.IconChanger;
using TrayGarden.Services.PlantServices.UserNotifications.Core.Plants;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Displaying;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications.Interfaces;
using TrayGarden.TypesHatcher;

namespace TrayGarden
{
  public class MockType : IPlant, IStandaloneIcon, INeedToModifyIcon, IExtendContextMenu, IExtendsGlobalMenu, IChangesGlobalIcon, IGetCustomSettingsStorage, IAskClipboardEvents, IServicesDelegation, IGetPowerOfUserNotifications
  {
    public bool Initialized { get; set; }
    public int IntValue { get; set; }
    public bool BoolValue { get; set; }
    public string StrValue { get; set; }

    public object ObjValue { get; set; }

    public List<int> IntList { get; set; }
    public List<string> StrList { get; set; }
    public List<MockType> ObjList { get; set; }

    public List<MockType> NullList { get; set; }

    public string Calculated { get; set; }

    public void Initialize()
    {
      Initialized = true;
    }

    public void PostServicesInitialize()
    {
      
    }

    private INotifyIconChangerClient NotifyIconChangerClient;

    public MockType()
    {
      IntList = new List<int>();
      StrList = new List<string>();
      Calculated = string.Empty;
      ObjList = new List<MockType>();
      HumanSupportingName = "Mock plant";
      Description = "customDesc";
    }

    public void MethodInt(int val)
    {
      Calculated += val;
    }

    public void MethodStr(string str)
    {
      Calculated += str;
    }

    public bool Method1(string someValue, object obj)
    {
      Calculated += string.Format("{{{0} - {1}}}", someValue, obj.ToString());
      return true;
    }

    public bool Method2(int someValue, object obj)
    {
      Calculated += string.Format("{{int{0} - {1}}}", someValue, obj.ToString());
      return true;
    }


    public bool GetIconInfo(out string title, out Icon icon, out MouseEventHandler iconClickHandler)
    {
      title = "Hello world";
      icon = HatcherGuide<IResourcesManager>.Instance.GetIconResource("mockIcon", null);
      iconClickHandler = IconClickHandler;
      return true;
    }

    private void IconClickHandler(object sender, MouseEventArgs mouseEventArgs)
    {
      if (mouseEventArgs.Button == MouseButtons.Left)
      {
        NotifyIconChangerClient.SetIcon(HatcherGuide<IResourcesManager>.Instance.GetIconResource("mockAction", null));
        IInformNotification informNotification =
          lord.CreateInformNotification("I'm here. Please speak with me.");
        INotificationResultCourier notificationResultCourier = lord.DisplayNotification(informNotification);
        NotificationResult resultWithWait = notificationResultCourier.GetResultWithWait();
        IInformNotification notification =
          lord.CreateInformNotification("I've received the " + resultWithWait.Code.ToString());
        lord.DisplayNotification(notification);
      }
    }

    public void StoreIconChangingAssignee(INotifyIconChangerClient notifyIconChangerClient)
    {
      NotifyIconChangerClient = notifyIconChangerClient;
    }

    public List<ToolStripMenuItem> GetStripsToAdd()
    {
      var items = new List<ToolStripMenuItem>();
      var item = new ToolStripMenuItem("something", null, OnClick);
      items.Add(item);
      item = new ToolStripMenuItem("something1", null, OnClick);

      items.Add(item);
      item = new ToolStripMenuItem("something1", null, OnClick);

      items.Add(item);
      return items;
    }

    private void OnClick(object sender, EventArgs eventArgs)
    {
      NotifyIconChangerClient.SetIcon(HatcherGuide<IResourcesManager>.Instance.GetIconResource("mockAction", null));
    }

    public bool GetMenuStripItemData(out string text, out Icon icon, out EventHandler clickHandler)
    {
      text = "Hello world";
      icon = HatcherGuide<IResourcesManager>.Instance.GetIconResource("mockIcon", null);
      clickHandler = (sender, args) => v2Changer.SetIcon(HatcherGuide<IResourcesManager>.Instance.GetIconResource("mockAction", null));
      return true;
    }


    private INotifyIconChangerClient v2Changer;
    public void StoreGlobalIconChangingAssignee(INotifyIconChangerClient notifyIconChangerClient)
    {
      v2Changer = notifyIconChangerClient;
    }

    private System.Configuration.Configuration modConfig;
    public void StoreModuleConfiguration(System.Configuration.Configuration moduleConfiguration)
    {
      modConfig = moduleConfiguration;
      var mainSection = modConfig.GetSection("trayGarden");
      int a = 19;
    }



    public void StoreCustomSettingsStorage(ISettingsBox settingsStorage)
    {
      int a = 10;
    }

    public void OnClipboardTextChanged(string newClipboardValue)
    {
      int a = 10;
    }

    public string HumanSupportingName { get; private set; }
    public string Description { get; private set; }

    public List<object> GetServiceDelegates()
    {
      return new List<object>
                {
                    new MockTypeUserConfig()
                };
    }


    private ILordOfNotifications lord;
    public void StoreLordOfNotifications(ILordOfNotifications lordOfNotifications)
    {
      lord = lordOfNotifications;
    }



  }

  class MockType2 : MockType
  {
  }

  class MockType3 : MockType2
  {
  }
}