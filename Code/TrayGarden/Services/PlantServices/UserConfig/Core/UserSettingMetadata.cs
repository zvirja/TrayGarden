using TrayGarden.Diagnostics;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

namespace TrayGarden.Services.PlantServices.UserConfig.Core;

public class UserSettingMetadata<T> : IUserSettingMetadataMaster<T>
{
  public virtual object AdditionalParams { get; protected set; }

  public T DefaultValue { get; protected set; }

  public string Description { get; private set; }

  public IUserSettingHallmark Hallmark { get; protected set; }

  public virtual string Name { get; protected set; }

  public virtual string Title { get; protected set; }

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