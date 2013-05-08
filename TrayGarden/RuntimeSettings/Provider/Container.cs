using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TrayGarden.Configuration;

namespace TrayGarden.RuntimeSettings.Provider
{
    [UsedImplicitly]
    public class Container : IContainer, ISupportPrototyping
    {
        protected Dictionary<string, string> Settings { get; set; }
        protected Dictionary<string, IContainer> InnerContainers { get; set; }
        public string Name { get; protected set; }


        #region Public methods

        public Container()
        {
            Settings = new Dictionary<string, string>();
            InnerContainers = new Dictionary<string, IContainer>();
        }

        public virtual void InitializeFromCollections(string name, Dictionary<string, string> settings,
                                                      IEnumerable<IContainer> subcontainers)
        {
            Name = name;
            foreach (KeyValuePair<string, string> stringStringPair in settings)
                Settings.Add(stringStringPair.Key, stringStringPair.Value);
            foreach (IContainer subcontainer in subcontainers)
            {
                InnerContainers.Add(subcontainer.Name, subcontainer);
            }
        }

        public virtual string GetStringSetting(string name)
        {
            if (Settings.ContainsKey(name))
                return Settings[name];
            return null;
        }

        public virtual void SetStringSetting(string name, string value)
        {
            SetStringSettingInternal(name, value);
        }

        public virtual IEnumerable<string> GetPresentStringSettingNames()
        {
            var res = Settings.Keys.ToList();
            return res;
        }

        public virtual IEnumerable<string> GetPresentSubContainerNames()
        {
            return InnerContainers.Keys.ToList();
        }

        public virtual IContainer GetNamedSubContainer(string name)
        {
            return ResolveNamedSubContainer(name);
        }

        public override string ToString()
        {
            return string.Format("{0} Settings:{1}, Inner:{2}", Name, Settings.Count, InnerContainers.Count);
        }

        public object CreateNewInializedInstance()
        {
            return new Container();
        }

        #endregion


        #region Protected methods

        protected virtual IContainer ResolveNamedSubContainer(string name)
        {
            if (InnerContainers.ContainsKey(name))
                return InnerContainers[name];
            var newContainer = new Container {Name = name};
            InnerContainers.Add(name, newContainer);
            return newContainer;
        }

        protected virtual void SetStringSettingInternal(string name, string value)
        {
            Settings[name] = value;
        }

        #endregion

    }
}