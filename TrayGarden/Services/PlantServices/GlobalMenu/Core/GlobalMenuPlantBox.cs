using System.Windows.Forms;
using TrayGarden.Plants;
using TrayGarden.RuntimeSettings;
using TrayGarden.Services.FleaMarket.IconChanger;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core
{
    public class GlobalMenuPlantBox : ServicePlantBoxBase
    {
        private INotifyIconChangerMaster _globalNotifyIconChanger;
        public ToolStripMenuItem ToolStripMenuItem { get; set; }
        public INotifyIconChangerMaster GlobalNotifyIconChanger
        {
            get
            {
                return _globalNotifyIconChanger;
            }
            set
            {
                _globalNotifyIconChanger = value;
                if (_globalNotifyIconChanger != null)
                    _globalNotifyIconChanger.IsEnabled = GlobalNotifyIconChangerEnabled;
            }
        }

        public bool GlobalNotifyIconChangerEnabled
        {
            get
            {
                bool isEnabled = SettingsBox.GetBool("notifyIconChangerEnabled", true);
                if (GlobalNotifyIconChanger != null)
                    GlobalNotifyIconChanger.IsEnabled = isEnabled;
                return isEnabled;
            }
            set
            {
                SettingsBox.SetBool("notifyIconChangerEnabled", value);
                if (GlobalNotifyIconChanger != null)
                    GlobalNotifyIconChanger.IsEnabled = value;
            }
        }

        public GlobalMenuPlantBox()
        {
            base.IsEnabledChanged += GlobalMenuPlantBox_IsEnabledChanged;
        }

        void GlobalMenuPlantBox_IsEnabledChanged(ServicePlantBoxBase sender, bool newValue)
        {
            FixVisibility();
        }

        public virtual void FixVisibility()
        {
            if (RelatedPlantEx.IsEnabled)
                ToolStripMenuItem.Visible = IsEnabled;
            else
                ToolStripMenuItem.Visible = false;
        }
    }
}
