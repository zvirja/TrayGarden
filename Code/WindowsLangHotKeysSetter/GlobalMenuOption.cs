using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsLangHotKeysSetter.Properties;
using TrayGarden.Resources;
using TrayGarden.Services.FleaMarket.IconChanger;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.ContextMenuCollecting;

namespace WindowsLangHotKeysSetter
{
  public class GlobalMenuOption: TrayGarden.Reception.Services.IExtendsGlobalMenu, TrayGarden.Reception.Services.IChangesGlobalIcon
  {
    public static GlobalMenuOption Instance = new GlobalMenuOption();

    protected INotifyIconChangerClient GlobalIconChanger { get; set; }

    public bool FillProvidedContextMenuBuilder(IMenuEntriesAppender menuAppender)
    {
      menuAppender.AppentMenuStripItem("Set Windows language hotkeys", Resources.activehotkeys, this.OnContextMenuClick);

      return true;
    }

    private void OnContextMenuClick(object sender, EventArgs e)
    {
      if (HotKeysCommandRunner.Instance.SetHotKeys(false))
      {
        this.GlobalIconChanger.NotifySuccess();
      }
    }

    public void StoreGlobalIconChangingAssignee(INotifyIconChangerClient notifyIconChangerClient)
    {
      this.GlobalIconChanger = notifyIconChangerClient;
    }

  }
}
