using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace TrayGarden.RuntimeSettings.Provider;

[UsedImplicitly]
public class Container : IContainer
{
  public Container()
  {
    Settings = new Dictionary<string, string>();
    InnerContainers = new Dictionary<string, IContainer>();
  }

  public string Name { get; private set; }

  private Dictionary<string, IContainer> InnerContainers { get; set; }

  private Dictionary<string, string> Settings { get; set; }

  public IContainer GetNamedSubContainer(string name)
  {
    return ResolveNamedSubContainer(name);
  }

  public IEnumerable<string> GetPresentStringSettingNames()
  {
    var res = Settings.Keys.ToList();
    return res;
  }

  public IEnumerable<string> GetPresentSubContainerNames()
  {
    return InnerContainers.Keys.ToList();
  }

  public string GetStringSetting(string name)
  {
    if (Settings.ContainsKey(name))
    {
      return Settings[name];
    }
    return null;
  }

  public void InitializeFromCollections(string name, Dictionary<string, string> settings, IEnumerable<IContainer> subcontainers)
  {
    Name = name;
    foreach (KeyValuePair<string, string> stringStringPair in settings)
    {
      Settings.Add(stringStringPair.Key, stringStringPair.Value);
    }
    foreach (IContainer subcontainer in subcontainers)
    {
      InnerContainers.Add(subcontainer.Name, subcontainer);
    }
  }

  public void SetStringSetting(string name, string value)
  {
    SetStringSettingInternal(name, value);
  }

  public override string ToString()
  {
    return string.Format("{0} Settings:{1}, Inner:{2}", Name, Settings.Count, InnerContainers.Count);
  }

  private IContainer ResolveNamedSubContainer(string name)
  {
    if (InnerContainers.ContainsKey(name))
    {
      return InnerContainers[name];
    }
    var newContainer = new Container { Name = name };
    InnerContainers.Add(name, newContainer);
    return newContainer;
  }

  private void SetStringSettingInternal(string name, string value)
  {
    Settings[name] = value;
  }
}