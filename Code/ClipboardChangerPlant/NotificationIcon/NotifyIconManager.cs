﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

using ClipboardChangerPlant.Configuration;
using ClipboardChangerPlant.Properties;

using TrayGarden.Reception.Services;
using TrayGarden.Reception.Services.StandaloneIcon;
using TrayGarden.Services.FleaMarket.IconChanger;

namespace ClipboardChangerPlant.NotificationIcon;

public class NotifyIconManager : INeedCongurationNode, IStandaloneIcon, INeedToModifyIcon, IExtendContextMenu, IChangesGlobalIcon
{
  private static readonly Lazy<NotifyIconManager> _manager =
    new Lazy<NotifyIconManager>(() => Factory.ActualFactory.GetNotifyIconManager());

  protected XmlHelper ConfigurationHelper;

  public event Action<object> MainActionRequested;

  public event Action<object> ShorteningRequested;

  public static NotifyIconManager ActualManager
  {
    get
    {
      return _manager.Value;
    }
  }

  public Icon DefaultTrayIcon
  {
    get
    {
      return ResourcesOperator.GetIconByName(ConfigurationHelper.GetStringValue("DefaultTrayIcon", "klipper"));
    }
  }

  public Icon ErrorTrayIcon
  {
    get
    {
      return ResourcesOperator.GetIconByName(ConfigurationHelper.GetStringValue("ErrorTrayIcon", "klipperFault"));
    }
  }

  public INotifyIconChangerClient GlobalNotifyIconChangerClient { get; set; }

  public Icon InProgressTrayIcon
  {
    get
    {
      return ResourcesOperator.GetIconByName(ConfigurationHelper.GetStringValue("InProgressTrayIcon", "klipperInProgress"));
    }
  }

  public string Name { get; set; }

  public Icon NotFoundTrayIcon
  {
    get
    {
      return ResourcesOperator.GetIconByName(ConfigurationHelper.GetStringValue("NotFoundTrayIcon", "klipperEmpty"));
    }
  }

  public INotifyIconChangerClient NotifyIconChangerClient { get; set; }

  public Icon SuccessTrayIcon
  {
    get
    {
      return ResourcesOperator.GetIconByName(ConfigurationHelper.GetStringValue("SuccessTrayIcon", "klipperSuccess"));
    }
  }

  public bool GetIconInfo(out string title, out Icon icon, out MouseEventHandler iconClickHandler)
  {
    title = "ClipboardChanger";
    icon = DefaultTrayIcon;
    iconClickHandler = NotifyIcon_MouseClick;
    return true;
  }

  public virtual List<ToolStripMenuItem> GetStripsToAdd()
  {
    var result = new List<ToolStripMenuItem>();
    var shortLink = new ToolStripMenuItem("Short link in clipboard", Resources.klipperShortedv5.ToBitmap());
    shortLink.Click += (sender, args) => OnShorteningRequested();
    result.Add(shortLink);
    return result;
  }

  public void SetConfigurationNode(XmlNode configurationNode)
  {
    ConfigurationHelper = new XmlHelper(configurationNode);
  }

  public virtual void SetNewIcon(Icon newIcon, int msTimeout = 0)
  {
    if (msTimeout == 0)
    {
      msTimeout = ConfigurationHelper.GetIntValue("DefaultTimeout", 400);
    }
    SetIconInternal(newIcon, msTimeout);
  }

  public virtual void StoreGlobalIconChangingAssignee(INotifyIconChangerClient notifyIconChangerClient)
  {
    GlobalNotifyIconChangerClient = notifyIconChangerClient;
  }

  public virtual void StoreIconChangingAssignee(INotifyIconChangerClient notifyIconChangerClient)
  {
    NotifyIconChangerClient = notifyIconChangerClient;
  }

  protected virtual void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
  {
    if (e.Button == MouseButtons.Left)
    {
      OnMainActionRequested();
    }
  }

  protected virtual void OnMainActionRequested()
  {
    Action<object> handler = MainActionRequested;
    if (handler != null)
    {
      handler(this);
    }
  }

  protected virtual void OnShorteningRequested()
  {
    Action<object> handler = ShorteningRequested;
    if (handler != null)
    {
      handler(this);
    }
  }

  protected virtual void SetIconInternal(Icon newIcon, int msTimeout)
  {
    NotifyIconChangerClient.SetIcon(newIcon, msTimeout);
  }
}