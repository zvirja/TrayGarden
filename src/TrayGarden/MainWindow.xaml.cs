using System.Windows;

using TrayGarden.Helpers.ThreadSwitcher;

namespace TrayGarden;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
  public MainWindow()
  {
    InitializeComponent();
    Closing += MainWindow_Closing;
  }

  private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
  {
  }
}

internal class IntSv : Switcher<int>
{
  public IntSv(int newValue)
    : base(newValue)
  {
  }
}
