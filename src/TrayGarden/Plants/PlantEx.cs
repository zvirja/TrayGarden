using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Reception;
using TrayGarden.RuntimeSettings;

namespace TrayGarden.Plants;

[UsedImplicitly]
public class PlantEx : IPlantEx
{
  public PlantEx()
  {
    Cloakroom = new Dictionary<string, object>();
  }

  public event PlantEnabledChangedEvent EnabledChanged;

  public string ID { get; private set; }

  public bool IsEnabled
  {
    get
    {
      AssertInitialized();
      return MySettingsBox.GetBool("enabled", false);
    }
    set
    {
      AssertInitialized();
      MySettingsBox.SetBool("enabled", value);
      OnEnabledChanged(this, value);
    }
  }

  public ISettingsBox MySettingsBox { get; private set; }

  public IPlant Plant { get; private set; }

  public List<object> Workhorses { get; private set; }

  private Dictionary<string, object> Cloakroom { get; set; }

  private bool Initialized { get; set; }

  public T GetFirstWorkhorseOfType<T>()
  {
    return (T)Workhorses.FirstOrDefault(x => x is T);
  }

  public object GetLuggage(string name)
  {
    AssertInitialized();
    if (!Cloakroom.ContainsKey(name))
    {
      return null;
    }
    return Cloakroom[name];
  }

  public T GetLuggage<T>(string name) where T : class
  {
    AssertInitialized();
    return GetLuggage(name) as T;
  }

  public bool HasLuggage(string name)
  {
    AssertInitialized();
    return Cloakroom.ContainsKey(name);
  }

  public void Initialize(
    [NotNull] IPlant plant,
    [NotNull] List<object> workhorses,
    [NotNull] string id,
    [NotNull] ISettingsBox mySettingsBox)
  {
    Assert.ArgumentNotNull(plant, "plant");
    Assert.ArgumentNotNull(workhorses, "workhorses");
    Assert.ArgumentNotNullOrEmpty(id, "id");
    Assert.ArgumentNotNull(mySettingsBox, "mySettingsBox");
    Workhorses = workhorses;
    Plant = plant;
    ID = id;
    MySettingsBox = mySettingsBox;
    Initialized = true;
  }

  public void PutLuggage(string name, object luggage)
  {
    AssertInitialized();
    Cloakroom[name] = luggage;
  }

  private void AssertInitialized()
  {
    if (!Initialized)
    {
      throw new NonInitializedException();
    }
  }

  private void OnEnabledChanged(IPlantEx plantEx, bool newValue)
  {
    PlantEnabledChangedEvent handler = EnabledChanged;
    if (handler != null)
    {
      handler(plantEx, newValue);
    }
  }
}