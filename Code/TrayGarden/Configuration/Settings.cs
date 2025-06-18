namespace TrayGarden.Configuration;

public static class Settings
{
  public static string ApplicationDataFolderName
  {
    get
    {
      return Factory.Instance.GetStringSetting("ApplicationData.FolderName", "TrayGarden");
    }
  }
}