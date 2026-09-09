using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows;

using JetBrains.Annotations;

using Microsoft.Extensions.Options;

using TrayGarden.Configuration;
using TrayGarden.Configuration.Options;
using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.Resources;
using TrayGarden.RuntimeSettings;
using TrayGarden.UI.Common.VMtoVMapping;

namespace TrayGarden.UI.WindowWithReturn;

/// <summary>
/// Interaction logic for WindowWithBack.xaml
/// </summary>
public partial class WindowWithBack : Window, IVMtoVMappingsSource, IWindowWithBack
{
  private static bool _exitOnClose;

  private readonly IResourcesManager _resourcesManager;

  private string iconResourceKey;

  private static ISettingsBox WindowWithBackSettingsBoxLazy;

  public WindowWithBack(IResourcesManager resourcesManager, IOptions<TrayGardenOptions> options, IEnumerable<IViewModelToViewMapping> mappings)
  {
    _resourcesManager = resourcesManager;
    Mappings = mappings.ToList();
    InitializeComponent();
    IconResourceKey = options.Value.WindowWithBack.IconResourceKey;
    StateToRestore = WindowState;
    Hide();
    SetIcon();
  }

  public static bool ExitOnClose
  {
    get
    {
      return WindowWithBackSettingsBox.GetBool("exitOnClose", false);
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

  private static ISettingsBox WindowWithBackSettingsBox
  {
    get
    {
      return WindowWithBackSettingsBoxLazy ??=
        GardenContext.RuntimeSettings.SystemSettings.GetSubBox("windowWithBackSettingsBox");
    }
  }

  private List<IViewModelToViewMapping> Mappings { get; set; }

  private WindowState StateToRestore { get; set; }

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

  public List<IViewModelToViewMapping> GetMappings()
  {
    return Mappings ?? new List<IViewModelToViewMapping>();
  }

  public void PrepareAndShow(WindowWithBackVM viewModel)
  {
    CleanupAndDisposeDataContext();
    DataContext = viewModel;
    viewModel.SizePozitionProvider = SizePozitionProvider;
    viewModel.PrepareToShow();
    SetSizeAndPos(viewModel);
    Show();
    WindowState = StateToRestore;
  }

  private void CleanupAndDisposeDataContext()
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

  private void SetIcon()
  {
    if (IconResourceKey.IsNullOrEmpty())
    {
      return;
    }
    Icon resource = _resourcesManager.GetIconResource(IconResourceKey, null);
    if (resource == null)
    {
      return;
    }
    Icon = ImageHelper.Bitmap2BitmapImage(resource.ToBitmap());
  }

  private void SetSizeAndPos(WindowWithBackVM viewModel)
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

  private bool SizePozitionProvider(out double top, out double left, out double width, out double height, out bool maximized)
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