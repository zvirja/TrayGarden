using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using TrayGarden.Reception.Services.StandaloneIcon;

namespace MicrosoftTodoTrayPlant;

public class TodoTrayIcon : IStandaloneIcon, IExtendContextMenu
{
  public static TodoTrayIcon Instance { get; } = new();

  public bool GetIconInfo(out string title, out Icon icon, out MouseEventHandler iconClickHandler)
  {
    title = "Microsoft To Do";
    icon = PlantResources.LoadTrayIcon();
    iconClickHandler = OnIconClick;
    return true;
  }

  public List<ToolStripMenuItem> GetStripsToAdd()
  {
    var showHide = new ToolStripMenuItem("Show / hide To Do");
    showHide.Click += (_, _) => WrapperController.Instance.ToggleVisibility();

    var closeApp = new ToolStripMenuItem("Quit To Do");
    closeApp.Click += (_, _) => WrapperController.Instance.CloseTarget();

    return new List<ToolStripMenuItem> { showHide, closeApp };
  }

  private static void OnIconClick(object sender, MouseEventArgs e)
  {
    if (e.Button == MouseButtons.Left)
    {
      WrapperController.Instance.ToggleVisibility();
    }
  }
}
