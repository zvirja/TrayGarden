using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

using CommunityToolkit.WinUI.Notifications;

using Windows.UI.Notifications;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core;

public interface IToastGate
{
  Task<string> ShowAsync(Toast toast, string originator, CancellationToken cancellationToken);

  void DiscardAll();
}

public sealed class ToastGate : IToastGate
{
  private const string ToastIdKey = "toast";
  private const string ButtonIdKey = "button";

  private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> _pending = new();

  public ToastGate()
  {
    ToastNotificationManagerCompat.OnActivated += OnActivated;
  }

  public Task<string> ShowAsync(Toast toast, string originator, CancellationToken cancellationToken)
  {
    string toastId = Guid.NewGuid().ToString("N");
    var completion = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
    _pending[toastId] = completion;

    try
    {
      ToastNotificationManagerCompat.CreateToastNotifier().Show(BuildNotification(toast, originator, toastId));
    }
    catch
    {
      _pending.TryRemove(toastId, out _);
      throw;
    }

    return WaitAsync(toastId, completion, toast.Timeout ?? Toast.DefaultTimeout, cancellationToken);
  }

  public void DiscardAll()
  {
    foreach (string toastId in _pending.Keys)
    {
      Complete(toastId, null);
    }
  }

  private ToastNotification BuildNotification(Toast toast, string originator, string toastId)
  {
    var builder = new ToastContentBuilder()
      .AddArgument(ToastIdKey, toastId)
      .AddAttributionText(originator)
      .AddText(toast.Title);

    if (toast.Body != null)
    {
      builder.AddText(toast.Body);
    }

    if (toast.Buttons is { Count: > 0 })
    {
      builder.SetToastScenario(ToastScenario.Reminder);
      foreach (ToastButton button in toast.Buttons)
      {
        builder.AddButton(
          new CommunityToolkit.WinUI.Notifications.ToastButton()
            .SetContent(button.Text)
            .AddArgument(ToastIdKey, toastId)
            .AddArgument(ButtonIdKey, button.Id)
            .SetBackgroundActivation());
      }
    }

    var notification = new ToastNotification(builder.GetToastContent().GetXml()) { Tag = toastId };
    notification.Dismissed += (_, _) => Complete(toastId, null, removeToast: false);
    notification.Failed += (_, _) => Complete(toastId, null, removeToast: false);
    return notification;
  }

  private async Task<string> WaitAsync(string toastId, TaskCompletionSource<string> completion, TimeSpan timeout, CancellationToken cancellationToken)
  {
    using var timeoutSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
    timeoutSource.CancelAfter(timeout);
    await using (timeoutSource.Token.Register(() => Complete(toastId, null)))
    {
      return await completion.Task;
    }
  }

  private void OnActivated(ToastNotificationActivatedEventArgsCompat args)
  {
    var arguments = ToastArguments.Parse(args.Argument);
    if (arguments.TryGetValue(ToastIdKey, out string toastId))
    {
      arguments.TryGetValue(ButtonIdKey, out string buttonId);
      Complete(toastId, buttonId);
    }
  }

  private void Complete(string toastId, string buttonId, bool removeToast = true)
  {
    if (!_pending.TryRemove(toastId, out TaskCompletionSource<string> completion))
    {
      return;
    }

    completion.TrySetResult(buttonId);
    if (removeToast)
    {
      try
      {
        ToastNotificationManagerCompat.History.Remove(toastId);
      }
      catch
      {
        // The toast may already be gone.
      }
    }
  }
}
