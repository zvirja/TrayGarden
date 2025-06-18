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
    positionAndSize = actualPositionAndSize;
    positionAndSize.Changed += PositionAndSizeOnChanged;
    NestedNotificationVM = nestedNotificationVM;
    NestedNotificationVM.ResultObtained += OnResultObtainedFromNestedNotification;
    isAlive = true;
    permanentCloseDescription = GetPermanentCloseDescription(originator);
    Result = new NotificationResult(ResultCode.Unspecified);

    CloseCommand = new RelayCommand(OnCloseCommandExecute, true);
    PermanentCloseCommand = new RelayCommand(OnPermanentlyCloseExecute, true);
  }

  public event PropertyChangedEventHandler PropertyChanged;

  public event EventHandler<ResultObtainedEventArgs> ResultObtained;

  public ICommand CloseCommand { get; set; }

  public virtual TimeSpan DelayBeforeForceClosing
  {
    get
    {
      return DelayBeforeForceFading + ForceFadingDuration.TimeSpan;
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
      return DelayBeforeNormalFading + NormalFadingDuration.TimeSpan;
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
      return isAlive;
    }
    set
    {
      if (value.Equals(isAlive))
      {
        return;
      }
      isAlive = value;
      OnPropertyChanged("IsAlive");
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
      return permanentCloseDescription;
    }
    set
    {
      if (value == permanentCloseDescription)
      {
        return;
      }
      permanentCloseDescription = value;
      OnPropertyChanged("PermanentCloseDescription");
    }
  }

  public virtual PositionSize PositionAndSize
  {
    get
    {
      return positionAndSize;
    }
    set
    {
      if (Equals(value, positionAndSize))
      {
        return;
      }
      positionAndSize = value;
      OnPropertyChanged("PositionAndSize");
    }
  }

  public NotificationResult Result { get; protected set; }

  public virtual void Dispose()
  {
    //Check whether this result was accepted. Otherwise no sense to inform listeners that we received it.
    bool arrivedOnTime = SetResultIfStillNeed(new NotificationResult(ResultCode.NoReaction));
    var nestedAsDisposable = NestedNotificationVM as IDisposable;
    if (nestedAsDisposable != null)
    {
      nestedAsDisposable.Dispose();
    }
    if (arrivedOnTime)
    {
      FireFireworkWeHaveResult();
    }
  }

  protected virtual void FireFireworkWeHaveResult()
  {
    OnResultObtained(Result);
  }

  protected string GetPermanentCloseDescription(string originator)
  {
    return UserNotificationsConfiguration.PermanentCloseDescriptionPattern.Value.FormatWith(originator);
  }

  protected virtual void OnCloseCommandExecute(object o)
  {
    SetResultIfStillNeed(new NotificationResult(ResultCode.Close));
    IsAlive = false;
    FireFireworkWeHaveResult();
  }

  protected virtual void OnPermanentlyCloseExecute(object obj)
  {
    SetResultIfStillNeed(new NotificationResult(ResultCode.PermanentlyClose));
    IsAlive = false;
    FireFireworkWeHaveResult();
  }

  [NotifyPropertyChangedInvocator]
  protected virtual void OnPropertyChanged(string propertyName)
  {
    PropertyChangedEventHandler handler = PropertyChanged;
    if (handler != null)
    {
      handler(this, new PropertyChangedEventArgs(propertyName));
    }
  }

  protected virtual void OnResultObtained(NotificationResult result)
  {
    EventHandler<ResultObtainedEventArgs> handler = ResultObtained;
    if (handler != null)
    {
      handler(this, new ResultObtainedEventArgs(result));
    }
  }

  protected virtual void OnResultObtainedFromNestedNotification(object sender, ResultObtainedEventArgs e)
  {
    SetResultIfStillNeed(e.Result);
    IsAlive = false;
    FireFireworkWeHaveResult();
  }

  protected virtual void PositionAndSizeOnChanged()
  {
    OnPropertyChanged("PositionAndSize");
  }

  protected virtual bool SetResultIfStillNeed(NotificationResult result)
  {
    if (Result.Code != ResultCode.Unspecified)
    {
      return false;
    }
    Result = result;
    return true;
  }
}