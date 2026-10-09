using System;
using System.Collections.Generic;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core;

/// <param name="Title">Bold first line.</param>
/// <param name="Body">Optional second line.</param>
/// <param name="Buttons">Optional action buttons. Their presence keeps the toast on screen until it is answered or times out.</param>
/// <param name="Timeout">How long to wait for an answer. <see cref="DefaultTimeout" /> when not set.</param>
public sealed record Toast(string Title, string Body = null, IReadOnlyList<ToastButton> Buttons = null, TimeSpan? Timeout = null)
{
  public static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);
}

/// <param name="Id">What <see cref="IUserNotifier.ShowAsync" /> returns when this button is clicked.</param>
public sealed record ToastButton(string Id, string Text);
