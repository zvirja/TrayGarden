namespace TrayGarden.Configuration.Options;

public class TrayGardenOptions
{
  public const string SectionName = "TrayGarden";

  public string AppDataFolderName { get; set; } = "TrayGarden";

  public string PlantsAutodetectFolder { get; set; } = "";

  public GlobalMenuOptions GlobalMenu { get; set; } = new();

  public NotifyIconChangerOptions NotifyIconChanger { get; set; } = new();

  public WindowWithBackOptions WindowWithBack { get; set; } = new();

  public RuntimeSettingsOptions RuntimeSettings { get; set; } = new();

  public ResourcesOptions Resources { get; set; } = new();
}

public class GlobalMenuOptions
{
  public string TrayIconResourceName { get; set; } = "gardenIconV2";

  public ContextMenuOptions ContextMenu { get; set; } = new();
}

public class ContextMenuOptions
{
  public bool BoldMainMenuEntries { get; set; }

  public bool ItalicMainMenuEntries { get; set; }

  public bool InsertDelimiterBetweenPlants { get; set; } = true;

  public string ConfigureIconResourceName { get; set; } = "configureV1";

  public string ExitIconResourceName { get; set; } = "exitIconV1";
}

public class NotifyIconChangerOptions
{
  public int DefaultDelayMsec { get; set; } = 500;
}

public class WindowWithBackOptions
{
  public string IconResourceKey { get; set; } = "gardenIconV3";
}

public class RuntimeSettingsOptions
{
  public int AutoSaveIntervalSeconds { get; set; }

  public StorageOptions Storage { get; set; } = new();
}

public class StorageOptions
{
  public string FileName { get; set; } = "RuntimeSettings.xml";

  public bool UseLocalFolder { get; set; } = false;

  public bool EnableDebuggingTraces { get; set; }
}

public class ResourcesOptions
{
  public ResourceSourceOptions[] Sources { get; set; } =
  [
    new() { Assembly = "TrayGarden", Path = "TrayGarden.Resources.GlobalResources" }
  ];
}

public class ResourceSourceOptions
{
  public string Assembly { get; set; }

  public string Path { get; set; }
}
