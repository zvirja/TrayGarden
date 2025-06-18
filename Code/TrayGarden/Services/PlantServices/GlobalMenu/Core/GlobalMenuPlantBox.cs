using System.Collections.Generic;
using System.Windows.Forms;

using TrayGarden.Services.FleaMarket.IconChanger;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core;

public class GlobalMenuPlantBox : ServicePlantBoxBase
{
  private INotifyIconChangerMaster _globalNotifyIconChanger;

  public GlobalMenuPlantBox()
  {
    IsEnabledChanged += GlobalMenuPlantBox_IsEnabledChanged;
  }

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
      {
        _globalNotifyIconChanger.IsEnabled = GlobalNotifyIconChangerEnabled;
      }
    }
  }

  public bool GlobalNotifyIconChangerEnabled
  {
    get
    {
      bool isEnabled = SettingsBox.GetBool("notifyIconChangerEnabled", true);
      if (GlobalNotifyIconChanger != null)
      {
        GlobalNotifyIconChanger.IsEnabled = isEnabled;
      }
      return isEnabled;
    }
    set
    {
      SettingsBox.SetBool("notifyIconChangerEnabled", value);
      if (GlobalNotifyIconChanger != null)
      {
        GlobalNotifyIconChanger.IsEnabled = value;
      }
    }
  }

  public bool IsGlobalIconChangingEnabled { get; set; }

  public List<ToolStripItem> ToolStripMenuItems { get; set; }

  public virtual void FixVisibility()
  {
    if (ToolStripMenuItems == null)
    {
      return;
    }
    var shouldBeVisible = RelatedPlantEx.IsEnabled && IsEnabled;
    foreach (ToolStripItem toolStripMenuItem in ToolStripMenuItems)
    {
      toolStripMenuItem.Visible = shouldBeVisible;
    }
  }

  private void GlobalMenuPlantBox_IsEnabledChanged(ServicePlantBoxBase sender, bool newValue)
  {
    FixVisibility();
  }
}