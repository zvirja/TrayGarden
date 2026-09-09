using TrayGarden.Diagnostics;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

namespace TrayGarden.Services.PlantServices.UserConfig.Core;

public class UserSettingMetadata<T> : IUserSettingMetadataMaster<T>
{
  public object AdditionalParams { get; private set; }

  public T DefaultValue { get; private set; }

  public string Description { get; private set; }

  public IUserSettingHallmark Hallmark { get; private set; }

  public string Name { get; private set; }

  public string Title { get; private set; }

  public void Initialize(
    string name,
    string title,
    T defaultValue,
    string description,
    object additionalParams,
    IUserSettingHallmark hallmark)
  {
    Assert.ArgumentNotNullOrEmpty(name, "name");
    Assert.ArgumentNotNullOrEmpty(title, "title");
    Name = name;
    Title = title;
    DefaultValue = defaultValue;
    Description = description;
    AdditionalParams = additionalParams;
    Hallmark = hallmark;
  }
}