#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.RuntimeSettings.Provider
{
  public interface IContainer
  {
    #region Public Properties

    string Name { get; }

    #endregion

    #region Public Methods and Operators

    IContainer GetNamedSubContainer(string name);

    IEnumerable<string> GetPresentStringSettingNames();

    IEnumerable<string> GetPresentSubContainerNames();

    string GetStringSetting(string name);

    void InitializeFromCollections(string name, Dictionary<string, string> settings, IEnumerable<IContainer> subcontainers);

    void SetStringSetting(string name, string value);

    #endregion
  }
}