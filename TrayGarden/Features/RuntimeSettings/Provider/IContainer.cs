using System.Collections.Generic;

namespace TrayGarden.Features.RuntimeSettings.Provider
{
    public interface IContainer
    {
        string Name { get; }

        string GetStringSetting(string name);
        void SetStringSetting(string name, string value);

        IEnumerable<string> GetPresentStringSettingNames();
        IEnumerable<string> GetPresentSubContainerNames();


        IContainer GetNamedSubContainer(string name);
        void InitializeFromCollections(string name, Dictionary<string, string> settings, IEnumerable<IContainer> subcontainers);
    }
}