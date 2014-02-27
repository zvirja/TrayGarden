#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Configuration;

#endregion

namespace TrayGarden.RuntimeSettings.Provider
{
  [UsedImplicitly]
  public class Container : IContainer, ISupportPrototyping
  {
    #region Constructors and Destructors

    public Container()
    {
      this.Settings = new Dictionary<string, string>();
      this.InnerContainers = new Dictionary<string, IContainer>();
    }

    #endregion

    #region Public Properties

    public string Name { get; protected set; }

    #endregion

    #region Properties

    protected Dictionary<string, IContainer> InnerContainers { get; set; }

    protected Dictionary<string, string> Settings { get; set; }

    #endregion

    #region Public Methods and Operators

    public object CreateNewInializedInstance()
    {
      return new Container();
    }

    public virtual IContainer GetNamedSubContainer(string name)
    {
      return this.ResolveNamedSubContainer(name);
    }

    public virtual IEnumerable<string> GetPresentStringSettingNames()
    {
      var res = this.Settings.Keys.ToList();
      return res;
    }

    public virtual IEnumerable<string> GetPresentSubContainerNames()
    {
      return this.InnerContainers.Keys.ToList();
    }

    public virtual string GetStringSetting(string name)
    {
      if (this.Settings.ContainsKey(name))
      {
        return this.Settings[name];
      }
      return null;
    }

    public virtual void InitializeFromCollections(string name, Dictionary<string, string> settings, IEnumerable<IContainer> subcontainers)
    {
      this.Name = name;
      foreach (KeyValuePair<string, string> stringStringPair in settings)
      {
        this.Settings.Add(stringStringPair.Key, stringStringPair.Value);
      }
      foreach (IContainer subcontainer in subcontainers)
      {
        this.InnerContainers.Add(subcontainer.Name, subcontainer);
      }
    }

    public virtual void SetStringSetting(string name, string value)
    {
      this.SetStringSettingInternal(name, value);
    }

    public override string ToString()
    {
      return string.Format("{0} Settings:{1}, Inner:{2}", this.Name, this.Settings.Count, this.InnerContainers.Count);
    }

    #endregion

    #region Methods

    protected virtual IContainer ResolveNamedSubContainer(string name)
    {
      if (this.InnerContainers.ContainsKey(name))
      {
        return this.InnerContainers[name];
      }
      var newContainer = new Container { Name = name };
      this.InnerContainers.Add(name, newContainer);
      return newContainer;
    }

    protected virtual void SetStringSettingInternal(string name, string value)
    {
      this.Settings[name] = value;
    }

    #endregion
  }
}