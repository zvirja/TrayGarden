namespace TrayGarden.TypesHatcher;

public static class HatcherGuide<TKeyInterface>
{
  public static TKeyInterface Instance
  {
    get
    {
      return (TKeyInterface)HatcherManager.Actual.GetObjectByType(typeof(TKeyInterface));
    }
  }

  public static TKeyInterface CreateNewInstance()
  {
    return (TKeyInterface)HatcherManager.Actual.GetNewObjectByType(typeof(TKeyInterface));
  }
}