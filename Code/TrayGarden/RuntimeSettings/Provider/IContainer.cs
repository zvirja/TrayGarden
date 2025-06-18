using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.RuntimeSettings.Provider;

public interface IContainer
{
  string Name { get; }

  IContainer GetNamedSubContainer(string name);

  IEnumerable<string> GetPresentStringSettingNames();

  IEnumerable<string> GetPresentSubContainerNames();

  string GetStringSetting(string name);

  void InitializeFromCollections(string name, Dictionary<string, string> settings, IEnumerable<IContainer> subcontainers);

  void SetStringSetting(string name, string value);
}