using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.Helpers.ThreadSwitcher;
using TrayGarden.RuntimeSettings;
using TrayGarden.TypesHatcher;
using TrayGarden.UI.Common.Commands;

namespace TrayGarden.UI.WindowWithReturn;

/// <summary>
/// ViewModel for Window with back
/// </summary>
public class WindowWithBackVM : INotifyPropertyChanged, IDisposable
{
  protected RelayCommand _backCommand;

  protected string _copyrightTitle;

  protected ObservableCollection<ActionCommandVM> _helpActions;

  private Stack<WindowStepState> _steps;

  public WindowWithBackVM()
  {
    this._backCommand = new RelayCommand(this.BackExecute, false);
    this._helpActions = new ObservableCollection<ActionCommandVM>();
    this._helpActions.CollectionChanged += this.HelpActions_CollectionChanged;
    this._copyrightTitle = "Zvirja Inc (c)";
    this._steps = new Stack<WindowStepState>();
    this.SelfSettingsBox = HatcherGuide<IRuntimeSettingsManager>.Instance.SystemSettings.GetSubBox("WindowWithBackVMBase");

    GoAheadTargets += this.GoAheadWithBack;
  }

  public delegate bool WindowSizePozitionInfoRetriever(
    out double top,
    out double left,
    out double width,
    out double height,
    out bool maximized);

  protected delegate void GoAheadWithBackInvokable(WindowStepState newState);

  public event PropertyChangedEventHandler PropertyChanged;

  protected static event GoAheadWithBackInvokable GoAheadTargets;

  [UsedImplicitly]
  public virtual RelayCommand BackCommand
  {
    get
    {
      return this._backCommand;
    }
  }

  public virtual string BackToTitle
  {
    get
    {
      return "Back to " + this.BackToTitleInternal;
    }
    set
    {
      if (value == this.BackToTitleInternal)
      {
        return;
      }
      this.BackToTitleInternal = value;
      this.OnPropertyChanged("BackToTitle");
    }
  }

  public virtual object ContentVM
  {
    get
    {
      return this.CurrentState.ContentVM;
    }
    set
    {
      if (this.CurrentState.ContentVM == value)
      {
        return;
      }
      this.CurrentState.ContentVM = value;
      this.OnPropertyChanged("ContentVM");
    }
  }

  [UsedImplicitly]
  public virtual string CopyrightTitle
  {
    get
    {
      return this._copyrightTitle;
    }
    set
    {
      if (value == this._copyrightTitle)
      {
        return;
      }
      this._copyrightTitle = value;
      this.OnPropertyChanged("CopyrightTitle");
    }
  }

  //--

  [UsedImplicitly]
  public virtual ICommand ExtraActionCommand
  {
    get
    {
      return this.CurrentState.SuperAction.Command;
    }
  }

  [UsedImplicitly]
  public virtual string ExtraActionTitle
  {
    get
    {
      return this.CurrentState.SuperAction.Title;
    }
  }

  public virtual string GlobalTitle
  {
    get
    {
      return this.CurrentState.GlobalTitle;
    }
  }

  public virtual string Header
  {
    get
    {
      return this.CurrentState.Header;
    }
  }

  public double Height
  {
    get
    {
      return GetDoubleValueOrZero(this.SelfSettingsBox.GetString("WindowHeight", null));
    }
    protected set
    {
      this.SelfSettingsBox.SetString("WindowHeight", value.ToString(CultureInfo.InvariantCulture));
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
      var aggregated = new List<ActionCommandVM>(this._helpActions);
      Assert.IsNotNull(this.CurrentState.StateSpecificHelpActions, "StateSpecificHelpActions can't be null");
      aggregated.AddRange(this.CurrentState.StateSpecificHelpActions);
      return aggregated;
    }
  }

  public double Left
  {
    get
    {
      return GetDoubleValueOrZero(this.SelfSettingsBox.GetString("WindowLeft", null));
    }
    protected set
    {
      this.SelfSettingsBox.SetString("WindowLeft", value.ToString(CultureInfo.InvariantCulture));
    }
  }

  public bool Maximized
  {
    get
    {
      return this.SelfSettingsBox.GetBool("WindowMaximized", false);
    }
    protected set
    {
      this.SelfSettingsBox.SetBool("WindowMaximized", value);
    }
  }

  /// <summary>
  /// Get actions, which are related to whole VM, not to current state.
  /// </summary>
  public ObservableCollection<ActionCommandVM> SelfHelpActions
  {
    get
    {
      return this._helpActions;
    }
  }

  public WindowSizePozitionInfoRetriever SizePozitionProvider { get; set; }

  public bool SizePropertiesAreValid
  {
    get
    {
      return this.SelfSettingsBox.GetBool("WindowPropertiesAreValid", false);
    }
    protected set
    {
      this.SelfSettingsBox.SetBool("WindowPropertiesAreValid", value);
    }
  }

  public double Top
  {
    get
    {
      return GetDoubleValueOrZero(this.SelfSettingsBox.GetString("WindowTop", null));
    }
    protected set
    {
      this.SelfSettingsBox.SetString("WindowTop", value.ToString(CultureInfo.InvariantCulture));
    }
  }

  public double Width
  {
    get
    {
      return GetDoubleValueOrZero(this.SelfSettingsBox.GetString("WindowWidth", null));
    }
    protected set
    {
      this.SelfSettingsBox.SetString("WindowWidth", value.ToString(CultureInfo.InvariantCulture));
    }
  }

  protected virtual string BackToTitleInternal { get; set; }

  protected virtual bool CanBack
  {
    get
    {
      return this._backCommand.CanExecute(null);
    }
    set
    {
      this._backCommand.CanExecuteMaster = value;
    }
  }

  protected virtual WindowStepState CurrentState
  {
    get
    {
      return this.Steps.Count > 0 ? this.Steps.Peek() : WindowStepState.EmptyState;
    }
  }

  protected ISettingsBox SelfSettingsBox { get; set; }

  protected virtual Stack<WindowStepState> Steps
  {
    get
    {
      return this._steps;
    }
    set
    {
      this._steps = value;
    }
  }

  protected int TimesEnterBulkUpdate { get; set; }

  public static void GoAheadWithBackIfPossible(WindowStepState newState)
  {
    if (GoAheadTargets != null)
    {
      GoAheadTargets(newState);
    }
  }

  public virtual void Dispose()
  {
    if (this.TimesEnterBulkUpdate == 1)
    {
      StackRawSwitcher<BulkUpdateState>.Exit();
      HatcherGuide<IRuntimeSettingsManager>.Instance.SaveNow(true);
      this.TimesEnterBulkUpdate--;
    }
    else
    {
      Log.Warn("Invalid value of TimesEnterBulkUpdate setting: {0}. Should be 1".FormatWith(this.TimesEnterBulkUpdate + 1), this);
    }
    GoAheadTargets -= this.GoAheadWithBack;
    this.ClearStepsStackWithDisposing();
  }

  public virtual void PrepareToShow()
  {
    StackRawSwitcher<BulkUpdateState>.Enter(BulkUpdateState.Enabled);
    this.TimesEnterBulkUpdate++;
    this._helpActions.Clear();
    foreach (ActionCommandVM helpAction in this.GetHelpActions())
    {
      this._helpActions.Add(helpAction);
    }
  }

  public virtual void ReplaceInitialState(WindowStepState newHomeState)
  {
    this.ClearStepsStackWithDisposing();
    this.Steps.Push(newHomeState);
    this.CanBack = false;
    this.NotifyPublicVisibleChanged();
  }

  protected static double GetDoubleValueOrZero(string str)
  {
    double result;
    return double.TryParse(str, out result) ? result : 0;
  }

  protected virtual void BackExecute(object o)
  {
    Assert.IsTrue(this.Steps.Count > 0, "Steps stack is corrupted. Can't be less than 1");
    var contentVMtoDestroy = this.ContentVM as IDisposable;
    if (contentVMtoDestroy != null)
    {
      contentVMtoDestroy.Dispose();
    }
    this.Steps.Pop();
    this.CanBack = this.Steps.Count > 1;
    if (this.CanBack)
    {
      WindowStepState windowStepState = this.Steps.Where((state, index) => index == 1).FirstOrDefault();
      if (windowStepState != null)
      {
        this.BackToTitle = windowStepState.ShortName;
      }
      else
      {
        this.BackToTitle = "hell :)";
      }
    }
    this.NotifyPublicVisibleChanged();
  }

  protected virtual void ClearStepsStackWithDisposing()
  {
    while (this.Steps.Count > 0)
    {
      WindowStepState currentStep = this.Steps.Pop();
      var currentStepContentVM = currentStep.ContentVM as IDisposable;
      if (currentStepContentVM != null)
      {
        currentStepContentVM.Dispose();
      }
    }
  }

  protected virtual List<ActionCommandVM> GetHelpActions()
  {
    return new List<ActionCommandVM>()
    {
      new ActionCommandVM(new RelayCommand(o => Application.Current.Shutdown(), true), "Close app"),
      new ActionCommandVM(new RelayCommand(this.SavePositionAndSize, true), "Save P&S")
    };
  }

  protected virtual void GoAheadWithBack(WindowStepState newState)
  {
    Assert.IsNotNull(newState, "New state cannot be null");
    this.BackToTitleInternal = this.CurrentState.ShortName;
    this.Steps.Push(newState);
    this.CanBack = true;
    this.NotifyPublicVisibleChanged();
  }

  protected virtual void HelpActions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
  {
    this.OnPropertyChanged("HelpActions");
  }

  protected virtual void NotifyPublicVisibleChanged()
  {
    this.OnPropertyChanged("GlobalTitle");
    this.OnPropertyChanged("Header");
    this.OnPropertyChanged("ContentVM");
    this.OnPropertyChanged("BackToTitle");
    this.OnPropertyChanged("ExtraActionTitle");
    this.OnPropertyChanged("ExtraActionCommand");
    this.OnPropertyChanged("HelpActions");
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

  protected virtual void SavePositionAndSize(object o)
  {
    if (this.SizePozitionProvider == null)
    {
      HatcherGuide<IUIManager>.Instance.OKMessageBox(
        "Tray Garden -- Save position and size",
        "Unable to save position and size. Provider is empty.");
      Log.Warn("SizePozitionProvider of WindowWithBackVMBase is empty. Something is wrong", this);
      return;
    }
    double top;
    double left;
    double width;
    double height;
    bool maximized;
    if (this.SizePozitionProvider(out top, out left, out width, out height, out maximized))
    {
      this.Top = top;
      this.Left = left;
      this.Width = width;
      this.Height = height;
      this.Maximized = maximized;
      this.SizePropertiesAreValid = true;
    }
  }
}