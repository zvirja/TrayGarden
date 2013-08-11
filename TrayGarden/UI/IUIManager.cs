using System;
using System.Windows;
using System.Windows.Threading;

namespace TrayGarden.UI
{
    public interface IUIManager
    {
        void ShowWindow(Window window);
        DispatcherOperation ShowWindowAsync(Window window);
        bool? ShowDialog(Window window);
        bool YesNoMessageBox(string caption, string text, MessageBoxImage image = MessageBoxImage.Question);

        void OKMessageBox(string caption, string text,
                                          MessageBoxImage image = MessageBoxImage.Information);

      void ExecuteActionOnUIThreadSynchronously(Action action);
      DispatcherOperation ExecuteActionOnUIThreadAsynchronously(Action action);
    }
}