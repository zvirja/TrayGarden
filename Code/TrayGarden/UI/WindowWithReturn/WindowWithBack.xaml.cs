using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.Resources;
using TrayGarden.RuntimeSettings;
using TrayGarden.TypesHatcher;
using TrayGarden.UI.Common.VMtoVMapping;

namespace TrayGarden.UI.WindowWithReturn
{
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
      this.InitializeComponent();
      this.IconResourceKey = "gardenIconV5";
      this.StateToRestore = this.WindowState;
      this.Hide();
      this.SetIcon();
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
        return this.iconResourceKey;
      }
      set
      {
        this.iconResourceKey = value;
        this.SetIcon();
      }
    }

    public bool IsCurrentlyDisplayed
    {
      get
      {
        return this.DataContext != null;
      }
    }

    protected static ISettingsBox WindowWithBackSettingsBox { get; set; }

    protected List<IViewModelToViewMapping> Mappings { get; set; }

    protected WindowState StateToRestore { get; set; }

    public void BringToFront()
    {
      if (this.IsCurrentlyDisplayed)
      {
        if (this.WindowState == WindowState.Minimized)
        {
          this.WindowState = WindowState.Normal;
        }
        this.Activate();
      }
    }

    public virtual List<IViewModelToViewMapping> GetMappings()
    {
      return this.Mappings ?? new List<IViewModelToViewMapping>();
    }

    public virtual void Initialize([NotNull] List<IViewModelToViewMapping> mvtovmappings)
    {
      Assert.ArgumentNotNull(mvtovmappings, "mvtovmappings");
      this.Mappings = mvtovmappings;
    }

    public virtual void PrepareAndShow(WindowWithBackVM viewModel)
    {
      this.CleanupAndDisposeDataContext();
      this.DataContext = viewModel;
      viewModel.SizePozitionProvider = this.SizePozitionProvider;
      viewModel.PrepareToShow();
      this.SetSizeAndPos(viewModel);
      this.Show();
      this.WindowState = this.StateToRestore;
    }

    protected virtual void CleanupAndDisposeDataContext()
    {
      var currentDataContextAsDisposable = this.DataContext as IDisposable;
      this.DataContext = null;
      if (currentDataContextAsDisposable != null)
      {
        currentDataContextAsDisposable.Dispose();
      }
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      e.Cancel = !ExitOnClose;
      this.Hide();

      this.CleanupAndDisposeDataContext();
      base.OnClosing(e);
    }

    protected override void OnStateChanged(EventArgs e)
    {
      if (this.WindowState == WindowState.Minimized && ExitOnClose)
      {
        this.Close();
      }
      else
      {
        this.StateToRestore = this.WindowState;
      }
      base.OnStateChanged(e);
    }

    protected void SetIcon()
    {
      if (this.IconResourceKey.IsNullOrEmpty())
      {
        return;
      }
      Icon resource = HatcherGuide<IResourcesManager>.Instance.GetIconResource(this.IconResourceKey, null);
      if (resource == null)
      {
        return;
      }
      this.Icon = ImageHelper.Bitmap2BitmapImage(resource.ToBitmap());
    }

    protected virtual void SetSizeAndPos(WindowWithBackVM viewModel)
    {
      if (!viewModel.SizePropertiesAreValid)
      {
        return;
      }
      this.Top = viewModel.Top;
      this.Left = viewModel.Left;
      this.Height = viewModel.Height;
      this.Width = viewModel.Width;
      this.WindowState = this.StateToRestore = viewModel.Maximized ? WindowState.Maximized : WindowState.Normal;
    }

    protected virtual bool SizePozitionProvider(out double top, out double left, out double width, out double height, out bool maximized)
    {
      if (this.WindowState == WindowState.Maximized)
      {
        top = this.RestoreBounds.Top;
        left = this.RestoreBounds.Left;
        height = this.RestoreBounds.Height;
        width = this.RestoreBounds.Width;
        maximized = true;
      }
      else
      {
        top = this.Top;
        left = this.Left;
        height = this.Height;
        width = this.Width;
        maximized = false;
      }
      return true;
    }
  }
}