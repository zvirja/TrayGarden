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
    public virtual DispatcherOperation ExecuteActionOnUIThreadAsynchronously(Action action)
    {
      return this.PerformOnDispatcherAsync(action);
    }

    public virtual void ExecuteActionOnUIThreadSynchronously(Action action)
    {
      this.PerformActionOnDispatcher(action);
    }

    public virtual void OKMessageBox(string caption, string text, MessageBoxImage image = MessageBoxImage.Information)
    {
      this.PerformActionOnDispatcher(() => MessageBox.Show(text, caption, MessageBoxButton.OK, image));
    }

    public virtual bool? ShowDialog(Window window)
    {
      return this.ShowDialogInternal(window);
    }

    public virtual void ShowWindow(Window window)
    {
      this.ShowWindowInternal(window);
    }

    public virtual DispatcherOperation ShowWindowAsync(Window window)
    {
      return this.ShowWindowInternalAsync(window);
    }

    public virtual bool YesNoMessageBox(string caption, string text, MessageBoxImage image = MessageBoxImage.Question)
    {
      var result = this.PerformOnDispatcher(new Func<MessageBoxResult>(() => MessageBox.Show(text, caption, MessageBoxButton.YesNo, image)));
      return ((MessageBoxResult)result) == MessageBoxResult.Yes;
    }

    protected virtual object PerformActionOnDispatcher(Action action)
    {
      return this.PerformOnDispatcher(action);
    }

    protected virtual object PerformActionWithParamOnDispatcher(Action<object> action, object parameter)
    {
      return this.PerformOnDispatcher(action, parameter);
    }

    protected virtual object PerformOnDispatcher(Delegate @delegate, object parameter)
    {
      return Application.Current.Dispatcher.Invoke(@delegate, DispatcherPriority.Input, parameter);
    }

    protected virtual object PerformOnDispatcher(Delegate @delegate)
    {
      return Application.Current.Dispatcher.Invoke(@delegate, DispatcherPriority.Input);
    }

    protected virtual DispatcherOperation PerformOnDispatcherAsync(Delegate @delegate, object parameter)
    {
      return Application.Current.Dispatcher.BeginInvoke(@delegate, DispatcherPriority.Input, parameter);
    }

    protected virtual DispatcherOperation PerformOnDispatcherAsync(Delegate @delegate)
    {
      return Application.Current.Dispatcher.BeginInvoke(@delegate, DispatcherPriority.Input);
    }

    protected virtual bool? ShowDialogInternal(Window window)
    {
      this.PerformActionWithParamOnDispatcher(this.ShowPassedDialog, window);
      return window.DialogResult;
    }

    protected virtual void ShowPassedDialog(object obj)
    {
      ((Window)obj).ShowDialog();
    }

    protected virtual void ShowPassedWindow(object obj)
    {
      ((Window)obj).Show();
    }

    protected virtual void ShowWindowInternal(Window window)
    {
      this.PerformActionWithParamOnDispatcher(this.ShowPassedWindow, window);
    }

    protected virtual DispatcherOperation ShowWindowInternalAsync(Window window)
    {
      return this.PerformOnDispatcherAsync(new Action<object>(this.ShowPassedWindow), window);
    }
  }
}