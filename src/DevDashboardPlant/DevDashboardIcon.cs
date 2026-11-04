using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using TrayGarden.Reception.Services.StandaloneIcon;

namespace DevDashboardPlant;

public class DevDashboardIcon : IStandaloneIcon, IExtendContextMenu
{
  public static DevDashboardIcon Instance { get; } = new();

  public bool GetIconInfo(out string title, out Icon icon, out MouseEventHandler iconClickHandler)
  {
    title = "Dev Dashboard";
    icon = DevDashboardResources.LoadTrayIcon();
    iconClickHandler = OnIconClick;
    return true;
  }

  public List<ToolStripMenuItem> GetStripsToAdd()
  {
    var toggle = new ToolStripMenuItem("Toggle Dev Dashboard");
    toggle.Click += (_, _) => DevDashboardController.Instance.Toggle();

    var reload = new ToolStripMenuItem("Reload Dev Dashboard");
    reload.Click += (_, _) => DevDashboardController.Instance.Reload(false);

    return [toggle, reload];
  }

  private static void OnIconClick(object sender, MouseEventArgs e)
  {
    if (e.Button == MouseButtons.Left)
    {
      DevDashboardController.Instance.Toggle();
    }
  }
}
