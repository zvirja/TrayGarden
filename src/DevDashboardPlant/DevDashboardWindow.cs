using System;
using System.ComponentModel;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Shell;
using System.Windows.Media;

using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;

using TrayGarden.Diagnostics;

namespace DevDashboardPlant;

/// <summary>
/// Hosts the site. Closing hides the window so the loaded page and its session survive until <see cref="CloseForReal"/>.
/// </summary>
internal sealed class DevDashboardWindow : Window
{
  private static readonly Color DarkBackground = Color.FromRgb(0x13, 0x15, 0x18);
  private static readonly Color DarkCaption = Color.FromRgb(0x19, 0x1D, 0x20);
  private const int DwmUseImmersiveDarkMode = 20;
  private const double CaptionHeight = 32;

  private static readonly string WebViewDataFolder = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
    "TrayGarden",
    "DevDashboard",
    "WebView2");

  private readonly WebView2 _webView = new();
  private readonly Grid _layout = new();
  private readonly Button _maximizeButton = CreateCaptionButton("", false);
  private const long TrayClickDeactivationWindowMs = 1000;

  private bool _closeForReal;
  private long _deactivatedAtTick;

  public DevDashboardWindow()
  {
    Title = "Dev Dashboard";
    Width = 1200;
    Height = 800;
    WindowStartupLocation = WindowStartupLocation.CenterScreen;
    UseLayoutRounding = true;
    SnapsToDevicePixels = true;
    Background = new SolidColorBrush(DarkBackground);
    _webView.DefaultBackgroundColor = System.Drawing.Color.FromArgb(DarkBackground.R, DarkBackground.G, DarkBackground.B);
    Icon = DevDashboardResources.LoadWindowIcon();

    // The page draws its own "Dev Dashboard" header, so the native caption (icon and title text) is replaced by a blank
    // strip. The native caption buttons are kept, and Window.Icon and Title still feed the taskbar and Alt+Tab.
    WindowChrome.SetWindowChrome(
      this,
      new WindowChrome
      {
        CaptionHeight = CaptionHeight,
        ResizeBorderThickness = new Thickness(6),
        GlassFrameThickness = new Thickness(0),
        UseAeroCaptionButtons = false
      });

    _layout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(CaptionHeight) });
    _layout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
    var captionStrip = new Border { Background = new SolidColorBrush(DarkCaption), Child = CreateCaptionButtons() };
    Grid.SetRow(_webView, 1);
    _layout.Children.Add(captionStrip);
    _layout.Children.Add(_webView);
    Content = _layout;
    StateChanged += (_, _) => OnWindowStateChanged();

    SourceInitialized += (_, _) => EnableDarkTitleBar();
    PreviewKeyDown += OnPreviewKeyDown;
    Loaded += async (_, _) => await InitializeWebViewAsync();
    PlantConfiguration.Instance.Url.ValueChanged += (_, _) => Dispatcher.BeginInvoke(NavigateToConfiguredUrl);
  }

  private UIElement CreateCaptionButtons()
  {
    Button minimize = CreateCaptionButton("", false);
    minimize.Click += (_, _) => WindowState = WindowState.Minimized;

    _maximizeButton.Click += (_, _) => WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;

    Button close = CreateCaptionButton("", true);
    close.Click += (_, _) => Close();

    var panel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };
    panel.Children.Add(minimize);
    panel.Children.Add(_maximizeButton);
    panel.Children.Add(close);
    WindowChrome.SetIsHitTestVisibleInChrome(panel, true);
    return panel;
  }

  private static Button CreateCaptionButton(string glyph, bool isClose)
  {
    string hover = isClose ? "#C42B1C" : "#2A2F33";
    var style = (Style)XamlReader.Parse(
      "<Style xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" TargetType=\"Button\">" +
      "<Setter Property=\"Template\"><Setter.Value><ControlTemplate TargetType=\"Button\">" +
      "<Border x:Name=\"root\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" Background=\"Transparent\">" +
      "<ContentPresenter HorizontalAlignment=\"Center\" VerticalAlignment=\"Center\"/></Border>" +
      "<ControlTemplate.Triggers><Trigger Property=\"IsMouseOver\" Value=\"True\">" +
      "<Setter TargetName=\"root\" Property=\"Background\" Value=\"" + hover + "\"/></Trigger></ControlTemplate.Triggers>" +
      "</ControlTemplate></Setter.Value></Setter></Style>");

    return new Button
    {
      Style = style,
      Width = 46,
      Height = CaptionHeight,
      Focusable = false,
      Foreground = Brushes.White,
      FontFamily = new FontFamily("Segoe Fluent Icons, Segoe MDL2 Assets"),
      FontSize = 10,
      Content = glyph
    };
  }

  /// <summary>
  /// A maximized window with custom chrome extends past the screen by its resize border, which pushes the page's
  /// scrollbar and edges off-screen unless the content is inset by the same amount.
  /// </summary>
  private void OnWindowStateChanged()
  {
    bool maximized = WindowState == WindowState.Maximized;
    _layout.Margin = maximized ? SystemParameters.WindowResizeBorderThickness : new Thickness(0);
    _maximizeButton.Content = maximized ? "" : "";
  }

  public void ShowAndActivate()
  {
    Show();
    if (WindowState == WindowState.Minimized)
    {
      WindowState = WindowState.Normal;
    }

    ForceToForeground();
    Activate();
    _webView.Focus();
  }

  /// <summary>
  /// A tray click moves focus to the taskbar before the click handler runs, so <see cref="Window.IsActive"/> is already false
  /// by then. The window counts as focused when it lost focus to the taskbar just now.
  /// </summary>
  public bool WasFocusedBeforeTrayClick()
  {
    return IsActive
           || (Environment.TickCount64 - _deactivatedAtTick < TrayClickDeactivationWindowMs && IsShellWindow(GetForegroundWindow()));
  }

  public void ReloadPage(bool bypassCache)
  {
    if (_webView.CoreWebView2 == null)
    {
      return;
    }

    if (bypassCache)
    {
      _ = _webView.CoreWebView2.CallDevToolsProtocolMethodAsync("Page.reload", "{\"ignoreCache\":true}");
    }
    else
    {
      _webView.Reload();
    }
  }

  public void CloseForReal()
  {
    _closeForReal = true;
    Close();
  }

  protected override void OnDeactivated(EventArgs e)
  {
    _deactivatedAtTick = Environment.TickCount64;
    base.OnDeactivated(e);
  }

  protected override void OnClosing(CancelEventArgs e)
  {
    if (!_closeForReal)
    {
      e.Cancel = true;
      Hide();
    }

    base.OnClosing(e);
  }

  protected override void OnClosed(EventArgs e)
  {
    _webView.Dispose();
    base.OnClosed(e);
  }

  private async Task InitializeWebViewAsync()
  {
    try
    {
      CoreWebView2Environment environment = await CoreWebView2Environment.CreateAsync(null, WebViewDataFolder);
      await _webView.EnsureCoreWebView2Async(environment);

      CoreWebView2 core = _webView.CoreWebView2;
      core.Profile.PreferredColorScheme = CoreWebView2PreferredColorScheme.Dark;
      core.DocumentTitleChanged += (_, _) => Title = string.IsNullOrWhiteSpace(core.DocumentTitle) ? "Dev Dashboard" : core.DocumentTitle;
      core.NewWindowRequested += OnNewWindowRequested;

      NavigateToConfiguredUrl();
    }
    catch (Exception ex)
    {
      Log.For(this).Error(ex, "Failed to initialize WebView2");
      MessageBox.Show(this, "Failed to initialize WebView2: " + ex.Message, "Dev Dashboard", MessageBoxButton.OK, MessageBoxImage.Error);
    }
  }

  private void NavigateToConfiguredUrl()
  {
    string url = PlantConfiguration.Instance.Url.Value;
    if (_webView.CoreWebView2 == null || !Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
    {
      return;
    }

    _webView.CoreWebView2.Navigate(uri.AbsoluteUri);
  }

  private void EnableDarkTitleBar()
  {
    IntPtr handle = new WindowInteropHelper(this).Handle;
    int enabled = 1;
    DwmSetWindowAttribute(handle, DwmUseImmersiveDarkMode, ref enabled, sizeof(int));

  }

  private static bool IsShellWindow(IntPtr hwnd)
  {
    if (hwnd == IntPtr.Zero)
    {
      return false;
    }

    GetWindowThreadProcessId(hwnd, out uint processId);
    try
    {
      using Process process = Process.GetProcessById((int)processId);
      return string.Equals(process.ProcessName, "explorer", StringComparison.OrdinalIgnoreCase);
    }
    catch (Exception)
    {
      return false;
    }
  }

  /// <summary>
  /// Windows refuses focus to a process that did not receive the last input, which is the case after a tray click.
  /// Attaching to the foreground thread's input queue lifts that restriction for the call.
  /// </summary>
  private void ForceToForeground()
  {
    IntPtr handle = new WindowInteropHelper(this).Handle;
    uint foregroundThread = GetWindowThreadProcessId(GetForegroundWindow(), out _);
    uint currentThread = GetCurrentThreadId();
    bool attached = foregroundThread != 0 && foregroundThread != currentThread && AttachThreadInput(currentThread, foregroundThread, true);
    SetForegroundWindow(handle);
    if (attached)
    {
      AttachThreadInput(currentThread, foregroundThread, false);
    }

    if (GetForegroundWindow() != handle)
    {
      SwitchToThisWindow(handle, true);
    }
  }

  [DllImport("user32.dll")]
  private static extern void SwitchToThisWindow(IntPtr hwnd, bool altTab);

  [DllImport("user32.dll")]
  private static extern IntPtr GetForegroundWindow();

  [DllImport("user32.dll")]
  private static extern bool SetForegroundWindow(IntPtr hwnd);

  [DllImport("user32.dll")]
  private static extern uint GetWindowThreadProcessId(IntPtr hwnd, out uint processId);

  [DllImport("user32.dll")]
  private static extern bool AttachThreadInput(uint attachThread, uint attachToThread, bool attach);

  [DllImport("kernel32.dll")]
  private static extern uint GetCurrentThreadId();

  [DllImport("dwmapi.dll")]
  private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attribute, ref int value, int size);

  private static void OnNewWindowRequested(object sender, CoreWebView2NewWindowRequestedEventArgs e)
  {
    e.Handled = true;
    Process.Start(new ProcessStartInfo(e.Uri) { UseShellExecute = true });
  }

  private void OnPreviewKeyDown(object sender, KeyEventArgs e)
  {
    if (e.Key == Key.F5)
    {
      ReloadPage((Keyboard.Modifiers & ModifierKeys.Control) != 0);
      e.Handled = true;
    }
  }
}
