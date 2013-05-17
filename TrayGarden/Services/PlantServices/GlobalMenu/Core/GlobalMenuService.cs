using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JetBrains.Annotations;
using TrayGarden.Plants;
using TrayGarden.Resources;
using TrayGarden.Services.FleaMarket.IconChanger;
using TrayGarden.TypesHatcher;

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
        public string IconResourceName { get; set; }

        public GlobalMenuService()
        {
            IconText = "Tray Garden";
            LuggageName = "GlobalMenuService";
            IconResourceName = "gardenIconV2";
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
            Icon iconResource = resourceManager.GetIconResource(IconResourceName, null);
            if (iconResource != null)
                return iconResource;
            var newIcon = new Bitmap(32, 32);
            IntPtr iconHandle = newIcon.GetHicon();
            iconResource = Icon.FromHandle(iconHandle);
            return iconResource;
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

        protected virtual void InitializePlantFromPipeline(IPlant plant)
        {
            INotifyIconChangerMaster globalNotifyIconChanger = HatcherGuide<INotifyIconChangerMaster>.CreateNewInstance();
            globalNotifyIconChanger.Initialize(GlobalNotifyIcon);
            InitPlantPipeline.InitPlantGMPipeline.Run(plant, LuggageName, globalNotifyIconChanger);
        }

        protected override void PlantOnEnabledChanged(IPlant plant, bool newValue)
        {
            GlobalMenuPlantBox plantBox = GetPlantLuggage(plant);
            if (plantBox != null)
                plantBox.IsEnabled = newValue;
        }


        protected virtual void BuildContextMenu(List<GlobalMenuPlantBox> plantBoxes)
        {
            var contextMenuStrip = new ContextMenuStrip();
            var configureItem = contextMenuStrip.Items.Add("Configure");
            configureItem.Click += ConfigureContextItemOnClick;
            contextMenuStrip.Items.Add("-");
            EnumeratePlantBoxes(plantBoxes, contextMenuStrip);
            contextMenuStrip.Items.Add("-");
            var exitItem = contextMenuStrip.Items.Add("Exit Garden");
            exitItem.Click += ExitContextItemOnClick;

            GlobalNotifyIcon.ContextMenuStrip = contextMenuStrip;
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


        public override void InitializePlant(IPlant plant)
        {
            base.InitializePlant(plant);
            InitializePlantFromPipeline(plant);
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
            List<IPlant> allPlants = HatcherGuide<IGardenbed>.Instance.GetAllPlants();
            foreach (IPlant plant in allPlants)
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
            GlobalNotifyIcon.Dispose();
        }

        #endregion

    }
}
