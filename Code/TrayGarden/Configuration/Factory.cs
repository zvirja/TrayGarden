namespace TrayGarden.Configuration;

public class Factory
{
  public static IFactory Instance
  {
    get
    {
      return ModernFactory.ActualInstance;
    }
  }
}