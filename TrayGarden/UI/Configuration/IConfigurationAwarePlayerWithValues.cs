using System.Windows.Input;

namespace TrayGarden.UI.Configuration
{
  public interface IConfigurationAwarePlayerWithValues : IConfigurationAwarePlayer
  {
    bool BoolValue { get; set; }
    int IntValue { get; set; }
    string StringValue { get; set; }
    string StringOptionValue { get; set; }
    object ObjectValue { get; set; }
    object StringOptions { get; }
    ICommand Action { get; }
    string ActionTitle { get; }
  }
}