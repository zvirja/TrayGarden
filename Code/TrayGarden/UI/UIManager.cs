using System;
using System.Windows;
using System.Windows.Threading;

namespace TrayGarden.UI;

public class UIManager : IUIManager
{
  public DispatcherOperation ExecuteActionOnUIThreadAsynchronously(Action action)
  {
    return PerformOnDispatcherAsync(action);
  }

  public void ExecuteActionOnUIThreadSynchronously(Action action)
  {
    PerformActionOnDispatcher(action);
  }

  public void OKMessageBox(string caption, string text, MessageBoxImage image = MessageBoxImage.Information)
  {
    PerformActionOnDispatcher(() => MessageBox.Show(text, caption, MessageBoxButton.OK, image));
  }

  public bool? ShowDialog(Window window)
  {
    return ShowDialogInternal(window);
  }

  public void ShowWindow(Window window)
  {
    ShowWindowInternal(window);
  }

  public DispatcherOperation ShowWindowAsync(Window window)
  {
    return ShowWindowInternalAsync(window);
  }

  public bool YesNoMessageBox(string caption, string text, MessageBoxImage image = MessageBoxImage.Question)
  {
    var result = PerformOnDispatcher(new Func<MessageBoxResult>(() => MessageBox.Show(text, caption, MessageBoxButton.YesNo, image)));
    return ((MessageBoxResult)result) == MessageBoxResult.Yes;
  }

  private object PerformActionOnDispatcher(Action action)
  {
    return PerformOnDispatcher(action);
  }

  private object PerformActionWithParamOnDispatcher(Action<object> action, object parameter)
  {
    return PerformOnDispatcher(action, parameter);
  }

  private object PerformOnDispatcher(Delegate @delegate, object parameter)
  {
    return Application.Current.Dispatcher.Invoke(@delegate, DispatcherPriority.Input, parameter);
  }

  private object PerformOnDispatcher(Delegate @delegate)
  {
    return Application.Current.Dispatcher.Invoke(@delegate, DispatcherPriority.Input);
  }

  private DispatcherOperation PerformOnDispatcherAsync(Delegate @delegate, object parameter)
  {
    return Application.Current.Dispatcher.BeginInvoke(@delegate, DispatcherPriority.Input, parameter);
  }

  private DispatcherOperation PerformOnDispatcherAsync(Delegate @delegate)
  {
    return Application.Current.Dispatcher.BeginInvoke(@delegate, DispatcherPriority.Input);
  }

  private bool? ShowDialogInternal(Window window)
  {
    PerformActionWithParamOnDispatcher(ShowPassedDialog, window);
    return window.DialogResult;
  }

  private void ShowPassedDialog(object obj)
  {
    ((Window)obj).ShowDialog();
  }

  private void ShowPassedWindow(object obj)
  {
    ((Window)obj).Show();
  }

  private void ShowWindowInternal(Window window)
  {
    PerformActionWithParamOnDispatcher(ShowPassedWindow, window);
  }

  private DispatcherOperation ShowWindowInternalAsync(Window window)
  {
    return PerformOnDispatcherAsync(new Action<object>(ShowPassedWindow), window);
  }
}