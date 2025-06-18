using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TrayGarden.Reception.Services.StandaloneIcon;

/// <summary>
/// This interface allows to provide service with self managed notify icon. 
/// The global attributes (such as visibility, life time) will be managed by service.
/// </summary>
public interface IAdvancedStandaloneIcon
{
  NotifyIcon GetNotifyIcon();
}