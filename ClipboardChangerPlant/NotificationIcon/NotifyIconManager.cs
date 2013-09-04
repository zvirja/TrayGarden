using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using ClipboardChangerPlant.Configuration;
using ClipboardChangerPlant.Properties;
using TrayGarden.Reception.Services;
using TrayGarden.Reception.Services.StandaloneIcon;
using TrayGarden.Services.FleaMarket.IconChanger;

namespace ClipboardChangerPlant.NotificationIcon
{
  public class NotifyIconManager : INeedCongurationNode, IStandaloneIcon, INeedToModifyIcon, IExtendContextMenu, IChangesGlobalIcon
  {
    private static readonly Lazy<NotifyIconManager> _manager = new Lazy<NotifyIconManager>(() => Factory.ActualFactory.GetNotifyIconManager());
    public static NotifyIconManager ActualManager
    {
      get
      {
        return _manager.Value;
      }
    }

    protected XmlHelper ConfigurationHelper;

    public INotifyIconChangerClient NotifyIconChangerClient { get; set; }
    public INotifyIconChangerClient GlobalNotifyIconChangerClient { get; set; }

    public Icon DefaultTrayIcon
    {
      get { return ResourcesOperator.GetIconByName(ConfigurationHelper.GetStringValue("DefaultTrayIcon")); }
    }

    public Icon NotFoundTrayIcon
    {
      get { return ResourcesOperator.GetIconByName(ConfigurationHelper.GetStringValue("NotFoundTrayIcon")); }
    }

    public Icon ErrorTrayIcon
    {
      get { return ResourcesOperator.GetIconByName(ConfigurationHelper.GetStringValue("ErrorTrayIcon")); }
    }

    public Icon SuccessTrayIcon
    {
      get { return ResourcesOperator.GetIconByName(ConfigurationHelper.GetStringValue("SuccessTrayIcon")); }
    }

    public Icon InProgressTrayIcon
    {
      get { return ResourcesOperator.GetIconByName(ConfigurationHelper.GetStringValue("InProgressTrayIcon")); }
    }

    public event Action<object> MainActionRequested;
    public event Action<object> ShorteningRequested;

    public bool GetIconInfo(out string title, out Icon icon, out MouseEventHandler iconClickHandler)
    {
      title = "ClipboardChanger";
      icon = DefaultTrayIcon;
      iconClickHandler = NotifyIcon_MouseClick;
      return true;
    }

    public virtual void StoreIconChangingAssignee(INotifyIconChangerClient notifyIconChangerClient)
    {
      NotifyIconChangerClient = notifyIconChangerClient;
    }


    public virtual List<ToolStripMenuItem> GetStripsToAdd()
    {
      var result = new List<ToolStripMenuItem>();
      var shortLink = new ToolStripMenuItem("Short link in clipboard", Resources.klipperShortedv5.ToBitmap());
      shortLink.Click +=(sender, args) => OnShorteningRequested();
      result.Add(shortLink);
      return result;
    }

    public virtual void StoreGlobalIconChangingAssignee(INotifyIconChangerClient notifyIconChangerClient)
    {
      GlobalNotifyIconChangerClient = notifyIconChangerClient;
    }

    public virtual void SetNewIcon(Icon newIcon, int msTimeout = 0)
    {
      if (msTimeout == 0)
        msTimeout = ConfigurationHelper.GetIntValue("DefaultTimeout");
      SetIconInternal(newIcon, msTimeout);
    }

    protected virtual void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
    {
      if(e.Button == MouseButtons.Left)
        OnMainActionRequested();
    }

    protected virtual void OnMainActionRequested()
    {
      Action<object> handler = MainActionRequested;
      if (handler != null) handler(this);
    }

    protected virtual void OnShorteningRequested()
    {
      Action<object> handler = ShorteningRequested;
      if (handler != null) handler(this);
    }

    protected virtual void SetIconInternal(Icon newIcon, int msTimeout)
    {
      NotifyIconChangerClient.SetIcon(newIcon,msTimeout);
    }



    #region Factory part

    public void SetConfigurationNode(XmlNode configurationNode)
    {
      ConfigurationHelper = new XmlHelper(configurationNode);
    }

    public string Name { get; set; }

    #endregion


  }
}
