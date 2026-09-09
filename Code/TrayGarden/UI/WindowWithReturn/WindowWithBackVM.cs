using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.Helpers.ThreadSwitcher;
using TrayGarden.RuntimeSettings;
using TrayGarden.UI.Common.Commands;

namespace TrayGarden.UI.WindowWithReturn;

/// <summary>
/// ViewModel for Window with back
/// </summary>
public class WindowWithBackVM : INotifyPropertyChanged, IDisposable
{
  private RelayCommand _backCommand;

  private string _copyrightTitle;

  private ObservableCollection<ActionCommandVM> _helpActions;

  private readonly IRuntimeSettingsManager _runtimeSettingsManager;

  private readonly IUIManager _uiManager;

  private Stack<WindowStepState> _steps;

  public WindowWithBackVM(IRuntimeSettingsManager runtimeSettingsManager, IUIManager uiManager)
  {
    _runtimeSettingsManager = runtimeSettingsManager;
    _uiManager = uiManager;
    _backCommand = new RelayCommand(BackExecute, false);
    _helpActions = new ObservableCollection<ActionCommandVM>();
    _helpActions.CollectionChanged += HelpActions_CollectionChanged;
    _copyrightTitle = "Zvirja Inc (c)";
    _steps = new Stack<WindowStepState>();
    SelfSettingsBox = _runtimeSettingsManager.SystemSettings.GetSubBox("WindowWithBackVMBase");

    GoAheadTargets += GoAheadWithBack;
  }

  public delegate bool WindowSizePozitionInfoRetriever(
    out double top,
    out double left,
    out double width,
    out double height,
    out bool maximized);

  private delegate void GoAheadWithBackInvokable(WindowStepState newState);

  public event PropertyChangedEventHandler PropertyChanged;

  private static event GoAheadWithBackInvokable GoAheadTargets;

  [UsedImplicitly]
  public RelayCommand BackCommand
  {
    get
    {
      return _backCommand;
    }
  }

  public string BackToTitle
  {
    get
    {
      return "Back to " + BackToTitleInternal;
    }
    set
    {
      if (value == BackToTitleInternal)
      {
        return;
      }
      BackToTitleInternal = value;
      OnPropertyChanged("BackToTitle");
    }
  }

  public object ContentVM
  {
    get
    {
      return CurrentState.ContentVM;
    }
    set
    {
      if (CurrentState.ContentVM == value)
      {
        return;
      }
      CurrentState.ContentVM = value;
      OnPropertyChanged("ContentVM");
    }
  }

  [UsedImplicitly]
  public string CopyrightTitle
  {
    get
    {
      return _copyrightTitle;
    }
    set
    {
      if (value == _copyrightTitle)
      {
        return;
      }
      _copyrightTitle = value;
      OnPropertyChanged("CopyrightTitle");
    }
  }

  //--

  [UsedImplicitly]
  public ICommand ExtraActionCommand
  {
    get
    {
      return CurrentState.SuperAction.Command;
    }
  }

  [UsedImplicitly]
  public string ExtraActionTitle
  {
    get
    {
      return CurrentState.SuperAction.Title;
    }
  }

  public string GlobalTitle
  {
    get
    {
      return CurrentState.GlobalTitle;
    }
  }

  public string Header
  {
    get
    {
      return CurrentState.Header;
    }
  }

  public double Height
  {
    get
    {
      return GetDoubleValueOrZero(SelfSettingsBox.GetString("WindowHeight", null));
    }
    private set
    {
      SelfSettingsBox.SetString("WindowHeight", value.ToString(CultureInfo.InvariantCulture));
    }
  }

  //---
  /// <summary>
  /// For binding. Returns aggregated collection. Don't use it to assign actions.
  /// </summary>
  public IEnumerable<ActionCommandVM> HelpActions
  {
    get
    {
      var aggregated = new List<ActionCommandVM>(_helpActions);
      Assert.IsNotNull(CurrentState.StateSpecificHelpActions, "StateSpecificHelpActions can't be null");
      aggregated.AddRange(CurrentState.StateSpecificHelpActions);
      return aggregated;
    }
  }

  public double Left
  {
    get
    {
      return GetDoubleValueOrZero(SelfSettingsBox.GetString("WindowLeft", null));
    }
    private set
    {
      SelfSettingsBox.SetString("WindowLeft", value.ToString(CultureInfo.InvariantCulture));
    }
  }

  public bool Maximized
  {
    get
    {
      return SelfSettingsBox.GetBool("WindowMaximized", false);
    }
    private set
    {
      SelfSettingsBox.SetBool("WindowMaximized", value);
    }
  }

  /// <summary>
  /// Get actions, which are related to whole VM, not to current state.
  /// </summary>
  public ObservableCollection<ActionCommandVM> SelfHelpActions
  {
    get
    {
      return _helpActions;
    }
  }

  public WindowSizePozitionInfoRetriever SizePozitionProvider { get; set; }

  public bool SizePropertiesAreValid
  {
    get
    {
      return SelfSettingsBox.GetBool("WindowPropertiesAreValid", false);
    }
    private set
    {
      SelfSettingsBox.SetBool("WindowPropertiesAreValid", value);
    }
  }

  public double Top
  {
    get
    {
      return GetDoubleValueOrZero(SelfSettingsBox.GetString("WindowTop", null));
    }
    private set
    {
      SelfSettingsBox.SetString("WindowTop", value.ToString(CultureInfo.InvariantCulture));
    }
  }

  public double Width
  {
    get
    {
      return GetDoubleValueOrZero(SelfSettingsBox.GetString("WindowWidth", null));
    }
    private set
    {
      SelfSettingsBox.SetString("WindowWidth", value.ToString(CultureInfo.InvariantCulture));
    }
  }

  private string BackToTitleInternal { get; set; }

  private bool CanBack
  {
    get
    {
      return _backCommand.CanExecute(null);
    }
    set
    {
      _backCommand.CanExecuteMaster = value;
    }
  }

  private WindowStepState CurrentState
  {
    get
    {
      return Steps.Count > 0 ? Steps.Peek() : WindowStepState.EmptyState;
    }
  }

  private ISettingsBox SelfSettingsBox { get; set; }

  private Stack<WindowStepState> Steps
  {
    get
    {
      return _steps;
    }
    set
    {
      _steps = value;
    }
  }

  private int TimesEnterBulkUpdate { get; set; }

  public static void GoAheadWithBackIfPossible(WindowStepState newState)
  {
    if (GoAheadTargets != null)
    {
      GoAheadTargets(newState);
    }
  }

  public void Dispose()
  {
    if (TimesEnterBulkUpdate == 1)
    {
      StackRawSwitcher<BulkUpdateState>.Exit();
      _runtimeSettingsManager.SaveNow(true);
      TimesEnterBulkUpdate--;
    }
    else
    {
      Log.For(this).Warning("Invalid value of TimesEnterBulkUpdate setting: {TimesEnterBulkUpdate}. Should be 1", TimesEnterBulkUpdate + 1);
    }
    GoAheadTargets -= GoAheadWithBack;
    ClearStepsStackWithDisposing();
  }

  public void PrepareToShow()
  {
    StackRawSwitcher<BulkUpdateState>.Enter(BulkUpdateState.Enabled);
    TimesEnterBulkUpdate++;
    _helpActions.Clear();
    foreach (ActionCommandVM helpAction in GetHelpActions())
    {
      _helpActions.Add(helpAction);
    }
  }

  public void ReplaceInitialState(WindowStepState newHomeState)
  {
    ClearStepsStackWithDisposing();
    Steps.Push(newHomeState);
    CanBack = false;
    NotifyPublicVisibleChanged();
  }

  private static double GetDoubleValueOrZero(string str)
  {
    double result;
    return double.TryParse(str, out result) ? result : 0;
  }

  private void BackExecute(object o)
  {
    Assert.IsTrue(Steps.Count > 0, "Steps stack is corrupted. Can't be less than 1");
    var contentVMtoDestroy = ContentVM as IDisposable;
    if (contentVMtoDestroy != null)
    {
      contentVMtoDestroy.Dispose();
    }
    Steps.Pop();
    CanBack = Steps.Count > 1;
    if (CanBack)
    {
      WindowStepState windowStepState = Steps.Where((state, index) => index == 1).FirstOrDefault();
      if (windowStepState != null)
      {
        BackToTitle = windowStepState.ShortName;
      }
      else
      {
        BackToTitle = "hell :)";
      }
    }
    NotifyPublicVisibleChanged();
  }

  private void ClearStepsStackWithDisposing()
  {
    while (Steps.Count > 0)
    {
      WindowStepState currentStep = Steps.Pop();
      var currentStepContentVM = currentStep.ContentVM as IDisposable;
      if (currentStepContentVM != null)
      {
        currentStepContentVM.Dispose();
      }
    }
  }

  private void CloseAppExecute(object o)
  {
    Log.For(this).Information("'Close app' help action invoked. Calling Application.Shutdown().");
    Application.Current.Shutdown();
  }

  private List<ActionCommandVM> GetHelpActions()
  {
    return new List<ActionCommandVM>()
    {
      new ActionCommandVM(new RelayCommand(CloseAppExecute, true), "Close app"),
      new ActionCommandVM(new RelayCommand(SavePositionAndSize, true), "Save P&S")
    };
  }

  private void GoAheadWithBack(WindowStepState newState)
  {
    Assert.IsNotNull(newState, "New state cannot be null");
    BackToTitleInternal = CurrentState.ShortName;
    Steps.Push(newState);
    CanBack = true;
    NotifyPublicVisibleChanged();
  }

  private void HelpActions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
  {
    OnPropertyChanged("HelpActions");
  }

  private void NotifyPublicVisibleChanged()
  {
    OnPropertyChanged("GlobalTitle");
    OnPropertyChanged("Header");
    OnPropertyChanged("ContentVM");
    OnPropertyChanged("BackToTitle");
    OnPropertyChanged("ExtraActionTitle");
    OnPropertyChanged("ExtraActionCommand");
    OnPropertyChanged("HelpActions");
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

  private void SavePositionAndSize(object o)
  {
    if (SizePozitionProvider == null)
    {
      _uiManager.OKMessageBox(
        "Tray Garden -- Save position and size",
        "Unable to save position and size. Provider is empty.");
      Log.For(this).Warning("SizePozitionProvider of WindowWithBackVMBase is empty. Something is wrong");
      return;
    }
    double top;
    double left;
    double width;
    double height;
    bool maximized;
    if (SizePozitionProvider(out top, out left, out width, out height, out maximized))
    {
      Top = top;
      Left = left;
      Width = width;
      Height = height;
      Maximized = maximized;
      SizePropertiesAreValid = true;
    }
  }
}