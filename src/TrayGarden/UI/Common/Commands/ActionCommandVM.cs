using System.Windows.Input;

namespace TrayGarden.UI.Common.Commands;

public class ActionCommandVM
{
  public ActionCommandVM(ICommand command, string title)
  {
    Command = command;
    Title = title;
  }

  public ICommand Command { get; private set; }

  public string Title { get; private set; }
}