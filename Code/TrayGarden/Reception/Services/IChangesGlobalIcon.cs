﻿using TrayGarden.Services.FleaMarket.IconChanger;

namespace TrayGarden.Reception.Services;

/// <summary>
/// This service allows to change the main notify icon.
/// </summary>
public interface IChangesGlobalIcon
{
  void StoreGlobalIconChangingAssignee(INotifyIconChangerClient notifyIconChangerClient);
}