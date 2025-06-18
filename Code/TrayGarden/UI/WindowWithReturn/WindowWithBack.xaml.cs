using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.Resources;
using TrayGarden.RuntimeSettings;
using TrayGarden.TypesHatcher;
using TrayGarden.UI.Common.VMtoVMapping;

namespace TrayGarden.UI.WindowWithReturn;

/// <summary>
/// Interaction logic for WindowWithBack.xaml
/// </summary>
public partial class WindowWithBack : Window, IVMtoVMappingsSource, IWindowWithBack
{
  protected static bool _exitOnClose;

  /*protected MappingsBasedContentValueConverter ViewModelToViewConverter { get; set; }*/

  protected string iconResourceKey;

  static WindowWithBack()
  {
    WindowWithBackSettingsBox = HatcherGuide<IRuntimeSettingsManager>.Instance.SystemSettings.GetSubBox("windowWithBackSettingsBox");
    _exitOnClose = WindowWithBackSettingsBox.GetBool("exitOnClose", false);
  }

  public WindowWithBack()
  {
    InitializeComponent();
    IconResourceKey = "gardenIconV5";
    StateToRestore = WindowState;
    Hide();
    SetIcon();
  }

  public static bool ExitOnClose
  {
    get
    {
      return _exitOnClose;
    }
    set
    {
      _exitOnClose = value;
      WindowWithBackSettingsBox.SetBool("exitOnClose", value);
    }
  }

  public string IconResourceKey
  {
    get
    {
      return iconResourceKey;
    }
    set
    {
      iconResourceKey = value;
      SetIcon();
    }
  }

  public bool IsCurrentlyDisplayed
  {
    get
    {
      return DataContext != null;
    }
  }

  protected static ISettingsBox WindowWithBackSettingsBox { get; set; }

  protected List<IViewModelToViewMapping> Mappings { get; set; }

  protected WindowState StateToRestore { get; set; }

  public void BringToFront()
  {
    if (IsCurrentlyDisplayed)
    {
      if (WindowState == WindowState.Minimized)
      {
        WindowState = WindowState.Normal;
      }
      Activate();
    }
  }

  public virtual List<IViewModelToViewMapping> GetMappings()
  {
    return Mappings ?? new List<IViewModelToViewMapping>();
  }

  public virtual void Initialize([NotNull] List<IViewModelToViewMapping> mvtovmappings)
  {
    Assert.ArgumentNotNull(mvtovmappings, "mvtovmappings");
    Mappings = mvtovmappings;
  }

  public virtual void PrepareAndShow(WindowWithBackVM viewModel)
  {
    CleanupAndDisposeDataContext();
    DataContext = viewModel;
    viewModel.SizePozitionProvider = SizePozitionProvider;
    viewModel.PrepareToShow();
    SetSizeAndPos(viewModel);
    Show();
    WindowState = StateToRestore;
  }

  protected virtual void CleanupAndDisposeDataContext()
  {
    var currentDataContextAsDisposable = DataContext as IDisposable;
    DataContext = null;
    if (currentDataContextAsDisposable != null)
    {
      currentDataContextAsDisposable.Dispose();
    }
  }

  protected override void OnClosing(CancelEventArgs e)
  {
    e.Cancel = !ExitOnClose;
    Hide();

    CleanupAndDisposeDataContext();
    base.OnClosing(e);
  }

  protected override void OnStateChanged(EventArgs e)
  {
    if (WindowState == WindowState.Minimized && ExitOnClose)
    {
      Close();
    }
    else
    {
      StateToRestore = WindowState;
    }
    base.OnStateChanged(e);
  }

  protected void SetIcon()
  {
    if (IconResourceKey.IsNullOrEmpty())
    {
      return;
    }
    Icon resource = HatcherGuide<IResourcesManager>.Instance.GetIconResource(IconResourceKey, null);
    if (resource == null)
    {
      return;
    }
    Icon = ImageHelper.Bitmap2BitmapImage(resource.ToBitmap());
  }

  protected virtual void SetSizeAndPos(WindowWithBackVM viewModel)
  {
    if (!viewModel.SizePropertiesAreValid)
    {
      return;
    }
    Top = viewModel.Top;
    Left = viewModel.Left;
    Height = viewModel.Height;
    Width = viewModel.Width;
    WindowState = StateToRestore = viewModel.Maximized ? WindowState.Maximized : WindowState.Normal;
  }

  protected virtual bool SizePozitionProvider(out double top, out double left, out double width, out double height, out bool maximized)
  {
    if (WindowState == WindowState.Maximized)
    {
      top = RestoreBounds.Top;
      left = RestoreBounds.Left;
      height = RestoreBounds.Height;
      width = RestoreBounds.Width;
      maximized = true;
    }
    else
    {
      top = Top;
      left = Left;
      height = Height;
      width = Width;
      maximized = false;
    }
    return true;
  }
}