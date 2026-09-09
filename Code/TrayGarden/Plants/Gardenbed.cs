using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

using TrayGarden.Configuration;
using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.Plants.Pipeline;
using TrayGarden.Reception;
using TrayGarden.RuntimeSettings;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Plants;

public class Gardenbed : IGardenbed
{
  public Gardenbed()
  {
    Plants = new Dictionary<string, IPlantEx>();
  }

  public virtual bool AutoDetectPlants
  {
    get
    {
      return MySettingsBox.GetBool("autoDetectPlants", true);
    }
    set
    {
      MySettingsBox.SetBool("autoDetectPlants", value);
    }
  }

  protected bool Initialized { get; set; }

  protected ISettingsBox MySettingsBox { get; set; }

  protected Dictionary<string, IPlantEx> Plants { get; set; }

  protected ISettingsBox RootPlantsSettingsBox
  {
    get
    {
      return MySettingsBox.GetSubBox("Plants");
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

  public virtual void InformPostInitStage()
  {
    foreach (IPlantEx plantEx in GetAllPlants())
    {
      plantEx.Plant.PostServicesInitialize();
    }
  }

  [UsedImplicitly]
  public virtual void Initialize(List<object> permanentPlants)
  {
    MySettingsBox = HatcherGuide<IRuntimeSettingsManager>.Instance.SystemSettings.GetSubBox("Gargedbed");
    if (permanentPlants == null)
    {
      permanentPlants = new List<object>();
    }
    permanentPlants.AddRange(GetAutoIncludePlants());
    foreach (object plant in permanentPlants)
    {
      IPlantEx resolvedPlantEx = ResolveIPlantEx(plant);
      if (resolvedPlantEx != null)
      {
        Plants.Add(resolvedPlantEx.ID, resolvedPlantEx);
      }
    }
    //HatcherGuide<IRuntimeSettingsManager>.Instance.SaveNow(false);
    Initialized = true;
  }

  protected virtual void AssertInitialized()
  {
    if (!Initialized)
    {
      throw new NonInitializedException();
    }
  }

  protected virtual DirectoryInfo GetAutoIncludeDirectory()
  {
    string folderSetting = Factory.Instance.GetStringSetting("Gardenbed.PlantsAutodetectFolder", string.Empty);
    string workingDirectory = DirectoryHelper.CurrentDirectory;
    Log.For(this).Debug("Gardenbed. CurrentDirectory: {WorkingDirectory}", workingDirectory);
    if (folderSetting.NotNullNotEmpty())
    {
      workingDirectory = Path.Combine(workingDirectory, folderSetting);
    }
    Log.For(this).Information("Gardenbed. Lookup directory: {WorkingDirectory}", workingDirectory);
    return new DirectoryInfo(workingDirectory);
  }

  protected virtual List<IPlant> GetAutoIncludePlants()
  {
    var result = new List<IPlant>();
    if (!AutoDetectPlants)
    {
      return result;
    }
    DirectoryInfo autoincludeDirectory = GetAutoIncludeDirectory();
    if (!autoincludeDirectory.Exists)
    {
      return result;
    }
    FileInfo[] assembliesFileInfos = autoincludeDirectory.GetFiles("*.dll", SearchOption.TopDirectoryOnly);
    foreach (FileInfo assemblyFileInfo in assembliesFileInfos)
    {
      // We do not want to load entry assembly one more time, as it's already loaded
      if (string.Equals(assemblyFileInfo.FullName, Assembly.GetEntryAssembly()?.Location))
      {
        continue;
      }
        
      List<IPlant> plantsInAssembly = GetPlantsFromAssemblyFile(assemblyFileInfo);
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
      Log.For(this).Information("We have found a suitable types in '{AssemblyFile}' file", assemblyFileInfo.FullName);
      var result = new List<IPlant>();
      foreach (Type candidate in candidates)
      {
        try
        {
          var instance = (IPlant)Activator.CreateInstance(candidate);
          Log.For(this).Information("Plant of type '{PlantType}' was successfully instantiated!", candidate.FullName);
          result.Add(instance);
        }
        catch (Exception ex)
        {
          Log.For(this).Error(ex, "Unable to instantiate IPlant of type '{PlantType}'", candidate.FullName);
        }
      }
      return result;
    }
    catch (Exception ex)
    {
      Log.For(this).Error(ex, "Unable to analyze file '{AssemblyFile}' for plants", assemblyFileInfo.FullName);
      return null;
    }
  }

  protected virtual IPlantEx ResolveIPlantEx(object plant)
  {
    var newPlant = InitializePlantExPipeline.Run(plant, RootPlantsSettingsBox);
    return newPlant;
  }
}