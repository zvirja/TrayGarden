#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Configuration;
using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.Plants.Pipeline;
using TrayGarden.Reception;
using TrayGarden.RuntimeSettings;
using TrayGarden.TypesHatcher;

#endregion

namespace TrayGarden.Plants
{
  public class Gardenbed : IGardenbed
  {
    #region Constructors and Destructors

    public Gardenbed()
    {
      this.Plants = new Dictionary<string, IPlantEx>();
    }

    #endregion

    #region Public Properties

    public virtual bool AutoDetectPlants
    {
      get
      {
        return this.MySettingsBox.GetBool("autoDetectPlants", true);
      }
      set
      {
        this.MySettingsBox.SetBool("autoDetectPlants", value);
      }
    }

    #endregion

    #region Properties

    protected bool Initialized { get; set; }

    protected ISettingsBox MySettingsBox { get; set; }

    protected Dictionary<string, IPlantEx> Plants { get; set; }

    protected ISettingsBox RootPlantsSettingsBox
    {
      get
      {
        return this.MySettingsBox.GetSubBox("Plants");
      }
    }

    #endregion

    #region Public Methods and Operators

    public virtual List<IPlantEx> GetAllPlants()
    {
      this.AssertInitialized();
      return this.Plants.Select(x => x.Value).ToList();
    }

    public virtual List<IPlantEx> GetEnabledPlants()
    {
      this.AssertInitialized();
      return this.Plants.Select(x => x.Value).Where(x => x.IsEnabled).ToList();
    }

    public virtual void InformPostInitStage()
    {
      foreach (IPlantEx plantEx in this.GetAllPlants())
      {
        plantEx.Plant.PostServicesInitialize();
      }
    }

    [UsedImplicitly]
    public virtual void Initialize(List<object> permanentPlants)
    {
      this.MySettingsBox = HatcherGuide<IRuntimeSettingsManager>.Instance.SystemSettings.GetSubBox("Gargedbed");
      if (permanentPlants == null)
      {
        permanentPlants = new List<object>();
      }
      permanentPlants.AddRange(this.GetAutoIncludePlants());
      foreach (object plant in permanentPlants)
      {
        IPlantEx resolvedPlantEx = this.ResolveIPlantEx(plant);
        if (resolvedPlantEx != null)
        {
          this.Plants.Add(resolvedPlantEx.ID, resolvedPlantEx);
        }
      }
      //HatcherGuide<IRuntimeSettingsManager>.Instance.SaveNow(false);
      this.Initialized = true;
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

    protected virtual DirectoryInfo GetAutoIncludeDirectory()
    {
      string folderSetting = Factory.Instance.GetStringSetting("Gardenbed.PlantsAutodetectFolder", string.Empty);
      string workingDirectory = DirectoryHelper.CurrentDirectory;
      Log.Debug("Gardenbed. CurrentDirectory: {0}".FormatWith(workingDirectory), this);
      if (folderSetting.NotNullNotEmpty())
      {
        workingDirectory = Path.Combine(workingDirectory, folderSetting);
      }
      Log.Info("Gardenbed. Lookup directory: {0}".FormatWith(workingDirectory), this);
      return new DirectoryInfo(workingDirectory);
    }

    protected virtual List<IPlant> GetAutoIncludePlants()
    {
      var result = new List<IPlant>();
      if (!this.AutoDetectPlants)
      {
        return result;
      }
      DirectoryInfo autoincludeDirectory = this.GetAutoIncludeDirectory();
      if (!autoincludeDirectory.Exists)
      {
        return result;
      }
      FileInfo[] assembliesFileInfos = autoincludeDirectory.GetFiles("*.dll", SearchOption.TopDirectoryOnly);
      if (assembliesFileInfos.Length == 0)
      {
        return result;
      }
      foreach (FileInfo assemblyFileInfo in assembliesFileInfos)
      {
        List<IPlant> plantsInAssembly = this.GetPlantsFromAssemblyFile(assemblyFileInfo);
        if (plantsInAssembly != null && plantsInAssembly.Count > 0)
        {
          result.AddRange(plantsInAssembly);
        }
      }
      return result;
    }

    protected virtual List<IPlant> GetPlantsFromAssemblyFile(FileInfo assemblyFileInfo)
    {
      try
      {
        Assembly assembly = Assembly.LoadFile(assemblyFileInfo.FullName);
        var candidates = assembly.GetTypes().Where(x => typeof(IPlant).IsAssignableFrom(x));
        if (!candidates.Any())
        {
          return null;
        }
        Log.Info("We have found a suitable types in '{0}' file".FormatWith(assemblyFileInfo.FullName), this);
        var result = new List<IPlant>();
        foreach (Type candidate in candidates)
        {
          try
          {
            var instance = (IPlant)Activator.CreateInstance(candidate);
            Log.Info("Plant of type '{0}' was successfully instantiated!".FormatWith(candidate.FullName), this);
            result.Add(instance);
          }
          catch (Exception ex)
          {
            Log.Error("Unable to instantiate IPlant of type '{0}'".FormatWith(candidate.FullName), ex, this);
          }
        }
        return result;
      }
      catch (Exception ex)
      {
        Log.Error("Unable to analyze file '{0}' for plants".FormatWith(assemblyFileInfo.FullName), ex, this);
        return null;
      }
    }

    protected virtual IPlantEx ResolveIPlantEx(object plant)
    {
      var newPlant = InitializePlantExPipeline.Run(plant, this.RootPlantsSettingsBox);
      return newPlant;
    }

    #endregion
  }
}