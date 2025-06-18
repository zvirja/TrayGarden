namespace TrayGarden.Configuration;

public class SimpleObjectFactory : IObjectFactory
{
  public string ConfigurationPath { get; set; }

  public virtual object GetObject()
  {
    return Factory.Instance.GetObject(ConfigurationPath);
  }

  public virtual object GetPurelyNewObject()
  {
    return Factory.Instance.GetPurelyNewObject(ConfigurationPath);
  }
}