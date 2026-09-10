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
  private bool isAlive;

  private string permanentCloseDescription;

  private PositionSize positionAndSize;

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

  public TimeSpan DelayBeforeForceClosing
  {
    get
    {
      return DelayBeforeForceFading + ForceFadingDuration.TimeSpan;
    }
  }

  public TimeSpan DelayBeforeForceFading
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

  public TimeSpan DelayBeforeNormalClosing
  {
    get
    {
      return DelayBeforeNormalFading + NormalFadingDuration.TimeSpan;
    }
  }

  public TimeSpan DelayBeforeNormalFading
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

  public bool DisplayPermanentlyCloseButton
  {
    get
    {
      return UserNotificationsConfiguration.DisplayPermanentCloseButton.Value;
    }
  }

  public Duration ForceFadingDuration
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
  public bool IsAlive
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

  public Duration NormalFadingDuration
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

  public string PermanentCloseDescription
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

  public PositionSize PositionAndSize
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

  public NotificationResult Result { get; private set; }

  public void Dispose()
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

  private void FireFireworkWeHaveResult()
  {
    OnResultObtained(Result);
  }

  private string GetPermanentCloseDescription(string originator)
  {
    return UserNotificationsConfiguration.PermanentCloseDescriptionPattern.Value.FormatWith(originator);
  }

  private void OnCloseCommandExecute(object o)
  {
    SetResultIfStillNeed(new NotificationResult(ResultCode.Close));
    IsAlive = false;
    FireFireworkWeHaveResult();
  }

  private void OnPermanentlyCloseExecute(object obj)
  {
    SetResultIfStillNeed(new NotificationResult(ResultCode.PermanentlyClose));
    IsAlive = false;
    FireFireworkWeHaveResult();
  }

  [NotifyPropertyChangedInvocator]
  private void OnPropertyChanged(string propertyName)
  {
    PropertyChangedEventHandler handler = PropertyChanged;
    if (handler != null)
    {
      handler(this, new PropertyChangedEventArgs(propertyName));
    }
  }

  private void OnResultObtained(NotificationResult result)
  {
    EventHandler<ResultObtainedEventArgs> handler = ResultObtained;
    if (handler != null)
    {
      handler(this, new ResultObtainedEventArgs(result));
    }
  }

  private void OnResultObtainedFromNestedNotification(object sender, ResultObtainedEventArgs e)
  {
    SetResultIfStillNeed(e.Result);
    IsAlive = false;
    FireFireworkWeHaveResult();
  }

  private void PositionAndSizeOnChanged()
  {
    OnPropertyChanged("PositionAndSize");
  }

  private bool SetResultIfStillNeed(NotificationResult result)
  {
    if (Result.Code != ResultCode.Unspecified)
    {
      return false;
    }
    Result = result;
    return true;
  }
}