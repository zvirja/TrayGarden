using TrayGarden.Configuration.ApplicationConfiguration.Autorun;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Configuration.ApplicationConfiguration;

public static class ActualAppProperties
{
  public static bool RunAtStartup
  {
    get
    {
      return HatcherGuide<IAutorunHelper>.Instance.IsAddedToAutorun;
    }
    set
    {
      HatcherGuide<IAutorunHelper>.Instance.SetNewAutorunValue(value);
    }
  }
}