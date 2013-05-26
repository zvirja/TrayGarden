using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media;
using JetBrains.Annotations;
using TrayGarden.Plants;
using TrayGarden.Resources;
using TrayGarden.Services.FleaMarket.IconChanger;
using TrayGarden.TypesHatcher;
using Color = System.Drawing.Color;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core
{
    [UsedImplicitly]
    public class GlobalMenuService:PlantServiceBase<GlobalMenuPlantBox>
    {
        private NotifyIcon _globalNotifyIcon;
        protected NotifyIcon GlobalNotifyIcon
        {
            get { return _globalNotifyIcon; }
            set { _globalNotifyIcon = value;
                int a = 10;
            }
        }

        public string IconText { get; set; }
        public string TrayIconResourceName { get; set; }
        public string ConfigureIconResourceName { get; set; }
        public string ExitIconResourceName { get; set; }
        public bool BoldMainMenuEntries { get; set; }
        public bool ItalicMainMenuEntries { get; set; }

        public GlobalMenuService()
        {
            IconText = "Tray Garden";
            LuggageName = "GlobalMenuService";
            TrayIconResourceName = "gardenIconV1";
            ConfigureIconResourceName = "configureV1";
            ExitIconResourceName = "exitIconV1";
            BoldMainMenuEntries = true;
            ItalicMainMenuEntries = true;
        }
       
       protected virtual void CreateNotifyIcon()
        {
            GlobalNotifyIcon = new NotifyIcon {Visible = false};
            GlobalNotifyIcon.Text = IconText;
            GlobalNotifyIcon.Icon = GetIcon();
            GlobalNotifyIcon.MouseClick += GlobalNotifyIcon_MouseClick;
        }

        protected virtual Icon GetIcon()
        {
            IResourcesManager resourceManager = HatcherGuide<IResourcesManager>.Instance;
            Icon iconResource = resourceManager.GetIconResource(TrayIconResourceName, null);
            if (iconResource != null)
                return iconResource;
            return GenerateIcon();
        }

        protected virtual Icon GenerateIcon()
        {
            var newIcon = new Bitmap(32, 32);
            var rand = new Random();
            for (int i = 0; i < 500; i++)
                newIcon.SetPixel(rand.Next(31), rand.Next(31), Color.YellowGreen);
            for (int i = 0; i < 250; i++)
                newIcon.SetPixel(rand.Next(31), rand.Next(31), Color.Tomato);
            IntPtr iconHandle = newIcon.GetHicon();
            return Icon.FromHandle(iconHandle);
        }

        protected virtual void GlobalNotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                OpenConfigurationWindow();
        }

        protected virtual void OpenConfigurationWindow()
        {
            //TODO IMPLEMENT LATER
        }

        protected virtual void InitializePlantFromPipeline(IPlantEx plantEx)
        {
            INotifyIconChangerMaster globalNotifyIconChanger = HatcherGuide<INotifyIconChangerMaster>.CreateNewInstance();
            globalNotifyIconChanger.Initialize(GlobalNotifyIcon);
            InitPlantPipeline.InitPlantGMPipeline.Run(plantEx, LuggageName, globalNotifyIconChanger);
        }

        protected override void PlantOnEnabledChanged(IPlantEx plantEx, bool newValue)
        {
            GlobalMenuPlantBox plantBox = GetPlantLuggage(plantEx);
            if (plantBox != null)
                plantBox.IsEnabled = newValue;
        }


        protected virtual void BuildContextMenu(List<GlobalMenuPlantBox> plantBoxes)
        {
            var contextMenuStrip = new ContextMenuStrip();
            BuildContextMenuPrefix(contextMenuStrip);
            EnumeratePlantBoxes(plantBoxes, contextMenuStrip);
            BuildContextMenuSuffix(contextMenuStrip);
            GlobalNotifyIcon.ContextMenuStrip = contextMenuStrip;
        }

        protected virtual void BuildContextMenuPrefix(ContextMenuStrip contextMenuStrip)
        {
            var configureItem = contextMenuStrip.Items.Add("Configure");
            Icon iconResource = HatcherGuide<IResourcesManager>.Instance.GetIconResource(ConfigureIconResourceName, null);
            if (iconResource != null)
                configureItem.Image = iconResource.ToBitmap();
            configureItem.Font = new Font(configureItem.Font, GetMainMenuEntriesStyle());
            configureItem.Click += ConfigureContextItemOnClick;
            contextMenuStrip.Items.Add("-");
        }

        protected virtual FontStyle GetMainMenuEntriesStyle()
        {
            FontStyle result = 0;
            if(BoldMainMenuEntries)
                result |=FontStyle.Bold;
            if(ItalicMainMenuEntries)
                result |=FontStyle.Italic;
            return result;
        }

        protected virtual void BuildContextMenuSuffix(ContextMenuStrip contextMenuStrip)
        {
            contextMenuStrip.Items.Add("-");
            var exitItem = contextMenuStrip.Items.Add("Exit Garden");
            Icon iconResource = HatcherGuide<IResourcesManager>.Instance.GetIconResource(ExitIconResourceName, null);
            if (iconResource != null)
                exitItem.Image = iconResource.ToBitmap();
            exitItem.Font = new Font(exitItem.Font, GetMainMenuEntriesStyle());
            exitItem.Click += ExitContextItemOnClick;
        }

        protected virtual void EnumeratePlantBoxes(List<GlobalMenuPlantBox> plantBoxes, ContextMenuStrip menuStrip)
        {
            foreach (GlobalMenuPlantBox globalMenuPlantBox in plantBoxes)
                menuStrip.Items.Add(globalMenuPlantBox.ToolStripMenuItem);
        }

        protected virtual void ExitContextItemOnClick(object sender, EventArgs eventArgs)
        {
            //TODO IMPLEMENT LATER
        }

        protected virtual void ConfigureContextItemOnClick(object sender, EventArgs eventArgs)
        {
            OpenConfigurationWindow();
        }

        #region Public methods


        public override void InitializePlant(IPlantEx plantEx)
        {
            base.InitializePlant(plantEx);
            InitializePlantFromPipeline(plantEx);
        }

        public override void InformInitializeStage()
        {
            base.InformInitializeStage();
            CreateNotifyIcon();
        }

        public override void InformDisplayStage()
        {
            base.InformDisplayStage();
            var plantBoxes = new List<GlobalMenuPlantBox>();
            List<IPlantEx> allPlants = HatcherGuide<IGardenbed>.Instance.GetAllPlants();
            foreach (IPlantEx plant in allPlants)
            {
                GlobalMenuPlantBox luggage = GetPlantLuggage(plant);
                if (luggage == null)
                    continue;
                plantBoxes.Add(luggage);
                luggage.FixVisibility();
            }

            BuildContextMenu(plantBoxes);
            GlobalNotifyIcon.Visible = true;
        }

        public override void InformClosingStage()
        {
            base.InformClosingStage();
            if(GlobalNotifyIcon != null)
                GlobalNotifyIcon.Dispose();
        }

        #endregion

    }
}
