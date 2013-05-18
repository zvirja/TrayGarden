using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TrayGarden.Configuration;
using TrayGarden.Resources;
using TrayGarden.RuntimeSettings;
using TrayGarden.Services.FleaMarket.IconChanger;
using TrayGarden.Services.PlantServices.ClipboardObserver.Smorgasbord;
using TrayGarden.Services.PlantServices.CustomSettings.Smorgasbord;
using TrayGarden.Services.PlantServices.GlobalMenu.Smorgasbord;
using TrayGarden.Services.PlantServices.MyAdminConfig.Smorgasbord;
using TrayGarden.Services.PlantServices.StandaloneIcon.Smorgasbord;
using TrayGarden.TypesHatcher;

namespace TrayGarden
{
    public class MockType : IStandaloneIcon, INeedToModifyIcon, IExtendContextMenu, IExtendsGlobalMenu, IChangesGlobalIcon, IGiveMeMyAppConfig, ISetCustomSettingsStorage, IAskClipboardEvents
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

        private INotifyIconChangerClient NotifyIconChangerClient;

        public MockType()
        {
            IntList = new List<int>();
            StrList = new List<string>();
            Calculated = string.Empty;
            ObjList = new List<MockType>();
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

        public bool Method2(int someValue,object obj)
        {
            Calculated += string.Format("{{int{0} - {1}}}",someValue,obj.ToString());
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
                NotifyIconChangerClient.SetIcon(HatcherGuide<IResourcesManager>.Instance.GetIconResource("mockAction",null));
            }
        }

        public void SetIconChangingAssignee(INotifyIconChangerClient notifyIconChangerClient)
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
        public void SetGlobalIconChangingAssignee(INotifyIconChangerClient notifyIconChangerClient)
        {
            v2Changer = notifyIconChangerClient;
        }

        private System.Configuration.Configuration modConfig;
        public void SetModuleConfiguration(System.Configuration.Configuration moduleConfiguration)
        {
            modConfig = moduleConfiguration;
            var mainSection = modConfig.GetSection("trayGarden");
            int a = 19;
        }


        
        public void SetCustomSettingsStorage(ISettingsBox settingsStorage)
        {
            int a = 10;
        }

        public void OnClipboardTextChanged(string newClipboardValue)
        {
            int a = 10;
        }
    }
}