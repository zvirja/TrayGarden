using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using TrayGarden.Configuration;
using TrayGarden.Diagnostics;
using TrayGarden.Plants.Pipeline;
using TrayGarden.Reception;
using TrayGarden.RuntimeSettings;
using TrayGarden.TypesHatcher;
using TrayGarden.Helpers;

namespace TrayGarden.Plants
{
  public class Gardenbed : IGardenbed
  {
    protected Dictionary<string, IPlantEx> Plants { get; set; }
    protected ISettingsBox MySettingsBox { get; set; }
    protected ISettingsBox RootPlantsSettingsBox
    {
      get { return MySettingsBox.GetSubBox("Plants"); }
    }
    protected bool Initialized { get; set; }

    public virtual bool AutoDetectPlants
    {
      get { return MySettingsBox.GetBool("autoDetectPlants", true); }
      set { MySettingsBox.SetBool("autoDetectPlants", value); }
    }


    public Gardenbed()
    {
      Plants = new Dictionary<string, IPlantEx>();
    }

    [UsedImplicitly]
    public virtual void Initialize(List<object> permanentPlants)
    {
      MySettingsBox = HatcherGuide<IRuntimeSettingsManager>.Instance.SystemSettings.GetSubBox("Gargedbed");
      if (permanentPlants == null)
        permanentPlants = new List<object>();
      permanentPlants.AddRange(GetAutoIncludePlants());
      foreach (object plant in permanentPlants)
      {
        IPlantEx resolvedPlantEx = ResolveIPlantEx(plant);
        if (resolvedPlantEx != null)
          Plants.Add(resolvedPlantEx.ID, resolvedPlantEx);
      }
      HatcherGuide<IRuntimeSettingsManager>.Instance.SaveNow(false);
      Initialized = true;
    }

    public virtual void InformPostInitStage()
    {
      foreach (IPlantEx plantEx in GetAllPlants())
      {
        plantEx.Plant.PostServicesInitialize();
      }
    }

    public virtual List<IPlantEx> GetAllPlants()
    {
      AssertInitialized();
      return Plants.Select(x => x.Value).ToList();
    }

    public virtual List<IPlantEx> GetEnabledPlants()
    {
      AssertInitialized();
      return Plants.Select(x => x.Value).Where(x => x.IsEnabled).ToList();
    }

    protected virtual IPlantEx ResolveIPlantEx(object plant)
    {
      var newPlant = InitializePlantExPipeline.Run(plant, RootPlantsSettingsBox);
      return newPlant;
    }

    protected virtual List<IPlant> GetAutoIncludePlants()
    {
      var result = new List<IPlant>();
      if (!AutoDetectPlants)
        return result;
      DirectoryInfo autoincludeDirectory = GetAutoIncludeDirectory();
      if (!autoincludeDirectory.Exists)
        return result;
      FileInfo[] assembliesFileInfos = autoincludeDirectory.GetFiles("*.dll", SearchOption.TopDirectoryOnly);
      if (assembliesFileInfos.Length == 0)
        return result;
      foreach (FileInfo assemblyFileInfo in assembliesFileInfos)
      {
        List<IPlant> plantsInAssembly = GetPlantsFromAssemblyFile(assemblyFileInfo);
        if (plantsInAssembly != null && plantsInAssembly.Count > 0)
          result.AddRange(plantsInAssembly);
      }
      return result;
    }

    protected virtual DirectoryInfo GetAutoIncludeDirectory()
    {
      string folderSetting = Factory.Instance.GetStringSetting("Gardenbed.PlantsAutodetectFolder",
                                                                           string.Empty);
      DirectoryInfo currentDirectory = new FileInfo(Assembly.GetEntryAssembly().Location).Directory;
      if (folderSetting.IsNullOrEmpty())
        return currentDirectory;
      else
        return new DirectoryInfo(Path.Combine(currentDirectory.FullName, folderSetting));
    }

    protected virtual List<IPlant> GetPlantsFromAssemblyFile(FileInfo assemblyFileInfo)
    {
      try
      {
        Assembly assembly = Assembly.LoadFile(assemblyFileInfo.FullName);
        var candidates = assembly.GetTypes().Where(x => typeof(IPlant).IsAssignableFrom(x));
        if (!candidates.Any())
          return null;
        Log.Info("We have found a suitable types in '{0}' file".FormatWith(assemblyFileInfo.FullName), this);
        var result = new List<IPlant>();
        foreach (Type candidate in candidates)
        {
          try
          {
            var instance = (IPlant)Activator.CreateInstance(candidate);
            Log.Info("Plant of type '{0}' was successfully instantiated!".FormatWith(candidate.FullName),
                     this);
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

    protected virtual void AssertInitialized()
    {
      if (!Initialized)
        throw new NonInitializedException();
    }

  }
}
