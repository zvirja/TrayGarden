using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;
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
    protected static ISettingsBox WindowWithBackSettingsBox { get; set; }
    protected static bool _exitOnClose;


    public static bool ExitOnClose
    {
      get { return _exitOnClose; }
      set
      {
        _exitOnClose = value;
        WindowWithBackSettingsBox.SetBool("exitOnClose", value);
      }
    }

    static WindowWithBack()
    {
      WindowWithBackSettingsBox =
          HatcherGuide<IRuntimeSettingsManager>.Instance.SystemSettings.GetSubBox("windowWithBackSettingsBox");
      _exitOnClose = WindowWithBackSettingsBox.GetBool("exitOnClose", false);
    }

    /*protected MappingsBasedContentValueConverter ViewModelToViewConverter { get; set; }*/
    protected List<IViewModelToViewMapping> Mappings { get; set; }
    protected WindowState StateToRestore { get; set; }


    public WindowWithBack()
    {

      InitializeComponent();
      StateToRestore = this.WindowState;
      Hide();
    }

    public virtual void Initialize([NotNull] List<IViewModelToViewMapping> mvtovmappings)
    {
      Assert.ArgumentNotNull(mvtovmappings, "mvtovmappings");
      Mappings = mvtovmappings;
    }

    public virtual void PrepareAndShow(WindowWithBackVM viewModel)
    {
      CleanupAndDisposeDataContext();
      this.DataContext = viewModel;
      viewModel.SizePozitionProvider = SizePozitionProvider;
      viewModel.PrepareToShow();
      SetSizeAndPos(viewModel);
      Show();
      WindowState = StateToRestore;
    }



    public virtual List<IViewModelToViewMapping> GetMappings()
    {
      return Mappings ?? new List<IViewModelToViewMapping>();
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
      if (this.WindowState == WindowState.Minimized && ExitOnClose)
      {
        Close();
      }
      else
      {
        StateToRestore = WindowState;
      }
      base.OnStateChanged(e);
    }

    protected virtual void CleanupAndDisposeDataContext()
    {
      var currentDataContextAsDisposable = DataContext as IDisposable;
      DataContext = null;
      if (currentDataContextAsDisposable != null)
        currentDataContextAsDisposable.Dispose();
    }

    protected virtual void SetSizeAndPos(WindowWithBackVM viewModel)
    {
      if (!viewModel.SizePropertiesAreValid)
        return;
      this.Top = viewModel.Top;
      this.Left = viewModel.Left;
      this.Height = viewModel.Height;
      this.Width = viewModel.Width;
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
}
