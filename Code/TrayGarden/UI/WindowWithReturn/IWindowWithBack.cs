namespace TrayGarden.UI.WindowWithReturn;

public interface IWindowWithBack
{
  bool IsCurrentlyDisplayed { get; }

  void BringToFront();

  void PrepareAndShow(WindowWithBackVM viewModel);
}
