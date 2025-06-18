namespace TrayGarden.Configuration;

public interface IObjectFactory
{
  object GetObject();

  object GetPurelyNewObject();
}