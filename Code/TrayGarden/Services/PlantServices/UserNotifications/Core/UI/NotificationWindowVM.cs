using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Positioning;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;
using TrayGarden.UI.Common.Commands;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI;

public class NotificationWindowVM : INotifyPropertyChanged, IDisposable, IResultProvider
{
  protected bool isAlive;

  protected string permanentCloseDescription;

  protected PositionSize positionAndSize;

  public NotificationWindowVM(
    [NotNull] PositionSize actualPositionAndSize,
    [NotNull] IResultProvider nestedNotificationVM,
    [NotNull] string originator)
  {
    Assert.ArgumentNotNull(actualPositionAndSize, "actualPositionAndSize");
    Assert.ArgumentNotNull(nestedNotificationVM, "nestedNotificationVM");
    Assert.ArgumentNotNullOrEmpty(originator, "originator");
    this.positionAndSize = actualPositionAndSize;
    this.positionAndSize.Changed += this.PositionAndSizeOnChanged;
    this.NestedNotificationVM = nestedNotificationVM;
    this.NestedNotificationVM.ResultObtained += this.OnResultObtainedFromNestedNotification;
    this.isAlive = true;
    this.permanentCloseDescription = this.GetPermanentCloseDescription(originator);
    this.Result = new NotificationResult(ResultCode.Unspecified);

    this.CloseCommand = new RelayCommand(this.OnCloseCommandExecute, true);
    this.PermanentCloseCommand = new RelayCommand(this.OnPermanentlyCloseExecute, true);
  }

  public event PropertyChangedEventHandler PropertyChanged;

  public event EventHandler<ResultObtainedEventArgs> ResultObtained;

  public ICommand CloseCommand { get; set; }

  public virtual TimeSpan DelayBeforeForceClosing
  {
    get
    {
      return this.DelayBeforeForceFading + this.ForceFadingDuration.TimeSpan;
    }
  }

  public virtual TimeSpan DelayBeforeForceFading
  {
    get
    {
      return UserNotificationsConfiguration.DelayBeforeForceFading.Value;
    }
    set
    {
      UserNotificationsConfiguration.DelayBeforeForceFading.Value = value;
    }
  }

  public virtual TimeSpan DelayBeforeNormalClosing
  {
    get
    {
      return this.DelayBeforeNormalFading + this.NormalFadingDuration.TimeSpan;
    }
  }

  public virtual TimeSpan DelayBeforeNormalFading
  {
    get
    {
      return UserNotificationsConfiguration.DelayBeforeNormalFading.Value;
    }
    set
    {
      UserNotificationsConfiguration.DelayBeforeNormalFading.Value = value;
    }
  }

  public virtual bool DisplayPermanentlyCloseButton
  {
    get
    {
      return UserNotificationsConfiguration.DisplayPermanentCloseButton.Value;
    }
  }

  public virtual Duration ForceFadingDuration
  {
    get
    {
      return UserNotificationsConfiguration.ForceFadingDuration.Value;
    }
    set
    {
      UserNotificationsConfiguration.ForceFadingDuration.Value = value.TimeSpan;
    }
  }

  [UsedImplicitly]
  public virtual bool IsAlive
  {
    get
    {
      return this.isAlive;
    }
    set
    {
      if (value.Equals(this.isAlive))
      {
        return;
      }
      this.isAlive = value;
      this.OnPropertyChanged("IsAlive");
    }
  }

  public bool IsPositionLocked { get; set; }

  public IResultProvider NestedNotificationVM { get; set; }

  public virtual Duration NormalFadingDuration
  {
    get
    {
      return UserNotificationsConfiguration.NormalFadingDuration.Value;
    }
    set
    {
      UserNotificationsConfiguration.NormalFadingDuration.Value = value.TimeSpan;
    }
  }

  public ICommand PermanentCloseCommand { get; set; }

  public virtual string PermanentCloseDescription
  {
    get
    {
      return this.permanentCloseDescription;
    }
    set
    {
      if (value == this.permanentCloseDescription)
      {
        return;
      }
      this.permanentCloseDescription = value;
      this.OnPropertyChanged("PermanentCloseDescription");
    }
  }

  public virtual PositionSize PositionAndSize
  {
    get
    {
      return this.positionAndSize;
    }
    set
    {
      if (Equals(value, this.positionAndSize))
      {
        return;
      }
      this.positionAndSize = value;
      this.OnPropertyChanged("PositionAndSize");
    }
  }

  public NotificationResult Result { get; protected set; }

  public virtual void Dispose()
  {
    //Check whether this result was accepted. Otherwise no sense to inform listeners that we received it.
    bool arrivedOnTime = this.SetResultIfStillNeed(new NotificationResult(ResultCode.NoReaction));
    var nestedAsDisposable = this.NestedNotificationVM as IDisposable;
    if (nestedAsDisposable != null)
    {
      nestedAsDisposable.Dispose();
    }
    if (arrivedOnTime)
    {
      this.FireFireworkWeHaveResult();
    }
  }

  protected virtual void FireFireworkWeHaveResult()
  {
    this.OnResultObtained(this.Result);
  }

  protected string GetPermanentCloseDescription(string originator)
  {
    return UserNotificationsConfiguration.PermanentCloseDescriptionPattern.Value.FormatWith(originator);
  }

  protected virtual void OnCloseCommandExecute(object o)
  {
    this.SetResultIfStillNeed(new NotificationResult(ResultCode.Close));
    this.IsAlive = false;
    this.FireFireworkWeHaveResult();
  }

  protected virtual void OnPermanentlyCloseExecute(object obj)
  {
    this.SetResultIfStillNeed(new NotificationResult(ResultCode.PermanentlyClose));
    this.IsAlive = false;
    this.FireFireworkWeHaveResult();
  }

  [NotifyPropertyChangedInvocator]
  protected virtual void OnPropertyChanged(string propertyName)
  {
    PropertyChangedEventHandler handler = this.PropertyChanged;
    if (handler != null)
    {
      handler(this, new PropertyChangedEventArgs(propertyName));
    }
  }

  protected virtual void OnResultObtained(NotificationResult result)
  {
    EventHandler<ResultObtainedEventArgs> handler = this.ResultObtained;
    if (handler != null)
    {
      handler(this, new ResultObtainedEventArgs(result));
    }
  }

  protected virtual void OnResultObtainedFromNestedNotification(object sender, ResultObtainedEventArgs e)
  {
    this.SetResultIfStillNeed(e.Result);
    this.IsAlive = false;
    this.FireFireworkWeHaveResult();
  }

  protected virtual void PositionAndSizeOnChanged()
  {
    this.OnPropertyChanged("PositionAndSize");
  }

  protected virtual bool SetResultIfStillNeed(NotificationResult result)
  {
    if (this.Result.Code != ResultCode.Unspecified)
    {
      return false;
    }
    this.Result = result;
    return true;
  }
}