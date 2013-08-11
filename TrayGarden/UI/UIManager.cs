using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace TrayGarden.UI
{
  public class UIManager : IUIManager
  {

    public virtual void ShowWindow(Window window)
    {
      ShowWindowInternal(window);
    }

    public virtual DispatcherOperation ShowWindowAsync(Window window)
    {
      return ShowWindowInternalAsync(window);
    }

    public virtual bool? ShowDialog(Window window)
    {
      return ShowDialogInternal(window);
    }

    public virtual bool YesNoMessageBox(string caption, string text, MessageBoxImage image = MessageBoxImage.Question)
    {
      var result = PerformOnDispatcher(new Func<MessageBoxResult>(
                                           () => MessageBox.Show(text, caption, MessageBoxButton.YesNo, image)));
      return ((MessageBoxResult)result) == MessageBoxResult.Yes;
    }


    public virtual void OKMessageBox(string caption, string text,
                                     MessageBoxImage image = MessageBoxImage.Information)
    {
      PerformActionOnDispatcher(() => MessageBox.Show(text, caption, MessageBoxButton.OK, image));
    }

    public virtual void ExecuteActionOnUIThreadSynchronously(Action action)
    {
      PerformActionOnDispatcher(action);
    }

    public virtual DispatcherOperation ExecuteActionOnUIThreadAsynchronously(Action action)
    {
      return PerformOnDispatcherAsync(action);
    }




    protected virtual void ShowWindowInternal(Window window)
    {
      PerformActionWithParamOnDispatcher(ShowPassedWindow, window);
    }

    protected virtual bool? ShowDialogInternal(Window window)
    {
      PerformActionWithParamOnDispatcher(ShowPassedDialog, window);
      return window.DialogResult;
    }

    protected virtual void ShowPassedWindow(object obj)
    {
      ((Window)obj).Show();
    }

    protected virtual void ShowPassedDialog(object obj)
    {
      ((Window)obj).ShowDialog();
    }

    protected virtual DispatcherOperation ShowWindowInternalAsync(Window window)
    {
      return PerformOnDispatcherAsync(new Action<object>(ShowPassedWindow), window);
    }

    protected virtual object PerformOnDispatcher(Delegate @delegate, object parameter)
    {
      return Application.Current.Dispatcher.Invoke(@delegate, DispatcherPriority.Input, parameter);
    }

    protected virtual object PerformOnDispatcher(Delegate @delegate)
    {
      return Application.Current.Dispatcher.Invoke(@delegate, DispatcherPriority.Input);
    }

    protected virtual object PerformActionWithParamOnDispatcher(Action<object> action, object parameter)
    {
      return PerformOnDispatcher(action, parameter);
    }

    protected virtual object PerformActionOnDispatcher(Action action)
    {
      return PerformOnDispatcher(action);
    }

    protected virtual DispatcherOperation PerformOnDispatcherAsync(Delegate @delegate, object parameter)
    {
      return Application.Current.Dispatcher.BeginInvoke(@delegate, DispatcherPriority.Input, parameter);
    }

    protected virtual DispatcherOperation PerformOnDispatcherAsync(Delegate @delegate)
    {
      return Application.Current.Dispatcher.BeginInvoke(@delegate, DispatcherPriority.Input);
    }
  }
}
