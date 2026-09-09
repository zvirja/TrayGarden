using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

using Microsoft.Extensions.Options;

using TrayGarden.Configuration.Options;
using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.Pipelines.Engine;
using TrayGarden.Plants.Pipeline;
using TrayGarden.Reception;
using TrayGarden.RuntimeSettings;

namespace TrayGarden.Plants;

public class Gardenbed : IGardenbed
{
  private readonly IRuntimeSettingsManager _runtimeSettingsManager;

  private readonly IPipelineRunner _pipelineRunner;

  private readonly string _plantsAutodetectFolder;

  public Gardenbed(IRuntimeSettingsManager runtimeSettingsManager, IPipelineRunner pipelineRunner, IOptions<TrayGardenOptions> options)
  {
    _runtimeSettingsManager = runtimeSettingsManager;
    _pipelineRunner = pipelineRunner;
    _plantsAutodetectFolder = options.Value.PlantsAutodetectFolder;
    Plants = new Dictionary<string, IPlantEx>();
  }

  public bool AutoDetectPlants
  {
    get
    {
      EnsureSettingsBox();
      return MySettingsBox.GetBool("autoDetectPlants", true);
    }
    set
    {
      EnsureSettingsBox();
      MySettingsBox.SetBool("autoDetectPlants", value);
    }
  }

  private bool Initialized { get; set; }

  private ISettingsBox MySettingsBox { get; set; }

  private Dictionary<string, IPlantEx> Plants { get; set; }

  private ISettingsBox RootPlantsSettingsBox
  {
    get
    {
      return MySettingsBox.GetSubBox("Plants");
    }
  }

  public List<IPlantEx> GetAllPlants()
  {
    EnsureInitialized();
    return Plants.Select(x => x.Value).ToList();
  }

  public List<IPlantEx> GetEnabledPlants()
  {
    EnsureInitialized();
    return Plants.Select(x => x.Value).Where(x => x.IsEnabled).ToList();
  }

  public void InformPostInitStage()
  {
    foreach (IPlantEx plantEx in GetAllPlants())
    {
      plantEx.Plant.PostServicesInitialize();
    }
  }

  private void EnsureSettingsBox()
  {
    MySettingsBox ??= _runtimeSettingsManager.SystemSettings.GetSubBox("Gargedbed");
  }

  private void EnsureInitialized()
  {
    if (Initialized)
    {
      return;
    }
    EnsureSettingsBox();
    var plants = new List<object>();
    plants.AddRange(GetAutoIncludePlants());
    foreach (object plant in plants)
    {
      IPlantEx resolvedPlantEx = ResolveIPlantEx(plant);
      if (resolvedPlantEx != null)
      {
        Plants.Add(resolvedPlantEx.ID, resolvedPlantEx);
      }
    }
    Initialized = true;
  }

  private DirectoryInfo GetAutoIncludeDirectory()
  {
    string folderSetting = _plantsAutodetectFolder ?? string.Empty;
    string workingDirectory = DirectoryHelper.CurrentDirectory;
    Log.For(this).Debug("Gardenbed. CurrentDirectory: {WorkingDirectory}", workingDirectory);
    if (folderSetting.NotNullNotEmpty())
    {
      workingDirectory = Path.Combine(workingDirectory, folderSetting);
    }
    Log.For(this).Information("Gardenbed. Lookup directory: {WorkingDirectory}", workingDirectory);
    return new DirectoryInfo(workingDirectory);
  }

  private List<IPlant> GetAutoIncludePlants()
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

  private List<IPlant> GetPlantsFromAssemblyFile(FileInfo assemblyFileInfo)
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

  private IPlantEx ResolveIPlantEx(object plant)
  {
    var args = new InitializePlantArgs(plant, RootPlantsSettingsBox);
    _pipelineRunner.Run(args);
    return args.ResolvedPlantEx;
  }
}