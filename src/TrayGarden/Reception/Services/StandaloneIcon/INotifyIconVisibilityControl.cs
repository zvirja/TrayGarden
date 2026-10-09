using System;

namespace TrayGarden.Reception.Services.StandaloneIcon;

/// <summary>
/// Lets a plant decide when its standalone notify icon is shown. The service hides the icon whenever the plant is
/// disabled, and otherwise shows it only while <see cref="IsIconVisible" /> is true. The plant raises
/// <see cref="IsIconVisibleChanged" /> (from any thread) after the value changes and never touches
/// <c>NotifyIcon.Visible</c> itself.
/// </summary>
public interface INotifyIconVisibilityControl
{
  event EventHandler IsIconVisibleChanged;

  bool IsIconVisible { get; }
}
