using System.Windows.Forms;
using TrayGarden.Plants;
using TrayGarden.RuntimeSettings;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core
{
    public class GlobalMenuPlantBox
    {
        public ISettingsBox SettingsBox { get; set; }
        public IPlantInternal RelatedPlantInternal { get; set; }
        public ToolStripMenuItem ToolStripMenuItem { get; set; }

        public virtual bool IsEnabled
        {
            get { return SettingsBox.GetBool("enabled", true); }
            set
            {
                SettingsBox.SetBool("enabled", value);
                FixVisibility();
            }
        }

        public virtual void FixVisibility()
        {
            if (RelatedPlantInternal.IsEnabled)
                ToolStripMenuItem.Visible = IsEnabled;
            else
                ToolStripMenuItem.Visible = false;
        }
    }
}
