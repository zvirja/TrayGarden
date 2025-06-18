using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace TrayGarden.UI;

public interface IUIManager
{
  DispatcherOperation ExecuteActionOnUIThreadAsynchronously(Action action);

  void ExecuteActionOnUIThreadSynchronously(Action action);

  void OKMessageBox(string caption, string text, MessageBoxImage image = MessageBoxImage.Information);

  bool? ShowDialog(Window window);

  void ShowWindow(Window window);

  DispatcherOperation ShowWindowAsync(Window window);

  bool YesNoMessageBox(string caption, string text, MessageBoxImage image = MessageBoxImage.Question);
}