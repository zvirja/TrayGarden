#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Reception;
using TrayGarden.RuntimeSettings;

#endregion

namespace TrayGarden.Plants
{
  [UsedImplicitly]
  public class PlantEx : IPlantEx
  {
    #region Constructors and Destructors

    public PlantEx()
    {
      this.Cloakroom = new Dictionary<string, object>();
    }

    #endregion

    #region Public Events

    public event PlantEnabledChangedEvent EnabledChanged;

    #endregion

    #region Public Properties

    public string ID { get; protected set; }

    public bool IsEnabled
    {
      get
      {
        this.AssertInitialized();
        return this.MySettingsBox.GetBool("enabled", false);
      }
      set
      {
        this.AssertInitialized();
        this.MySettingsBox.SetBool("enabled", value);
        this.OnEnabledChanged(this, value);
      }
    }

    public ISettingsBox MySettingsBox { get; protected set; }

    public IPlant Plant { get; protected set; }

    public List<object> Workhorses { get; protected set; }

    #endregion

    #region Properties

    protected Dictionary<string, object> Cloakroom { get; set; }

    protected bool Initialized { get; set; }

    #endregion

    #region Public Methods and Operators

    public T GetFirstWorkhorseOfType<T>()
    {
      return (T)this.Workhorses.FirstOrDefault(x => x is T);
    }

    public virtual object GetLuggage(string name)
    {
      this.AssertInitialized();
      if (!this.Cloakroom.ContainsKey(name))
      {
        return null;
      }
      return this.Cloakroom[name];
    }

    public virtual T GetLuggage<T>(string name) where T : class
    {
      this.AssertInitialized();
      return this.GetLuggage(name) as T;
    }

    public virtual bool HasLuggage(string name)
    {
      this.AssertInitialized();
      return this.Cloakroom.ContainsKey(name);
    }

    public virtual void Initialize(
      [NotNull] IPlant plant,
      [NotNull] List<object> workhorses,
      [NotNull] string id,
      [NotNull] ISettingsBox mySettingsBox)
    {
      Assert.ArgumentNotNull(plant, "plant");
      Assert.ArgumentNotNull(workhorses, "workhorses");
      Assert.ArgumentNotNullOrEmpty(id, "id");
      Assert.ArgumentNotNull(mySettingsBox, "mySettingsBox");
      this.Workhorses = workhorses;
      this.Plant = plant;
      this.ID = id;
      this.MySettingsBox = mySettingsBox;
      this.Initialized = true;
    }

    public virtual void PutLuggage(string name, object luggage)
    {
      this.AssertInitialized();
      this.Cloakroom[name] = luggage;
    }

    #endregion

    #region Methods

    protected virtual void AssertInitialized()
    {
      if (!this.Initialized)
      {
        throw new NonInitializedException();
      }
    }

    protected virtual void OnEnabledChanged(IPlantEx plantEx, bool newValue)
    {
      PlantEnabledChangedEvent handler = this.EnabledChanged;
      if (handler != null)
      {
        handler(plantEx, newValue);
      }
    }

    #endregion
  }
}