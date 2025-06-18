using System.Windows.Controls;

namespace TrayGarden.UI.Common.VMtoVMapping;

public interface ISelfViewResolver
{
  Control GetViewToPresentMe();
}