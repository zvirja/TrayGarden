using System.Threading;
using System.Threading.Tasks;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core;

public interface IUserNotifier
{
  /// <returns>
  /// The <see cref="ToastButton.Id" /> of the clicked button, or null when the toast was clicked, dismissed, timed out,
  /// or notifications are disabled for the plant. Never throws for those cases.
  /// </returns>
  Task<string> ShowAsync(Toast toast, CancellationToken cancellationToken = default);
}
