﻿using System.Windows.Forms;

using JetBrains.Annotations;

namespace TrayGarden.Services.FleaMarket.IconChanger;

public interface INotifyIconChangerMaster : INotifyIconChangerClient
{
  int DefaultDelayMsec { get; set; }

  bool IsEnabled { get; set; }

  void Initialize([NotNull] NotifyIcon operableNIcon);
}