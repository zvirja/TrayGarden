using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

using Microsoft.Extensions.DependencyModel;

using TrayGarden.Diagnostics;
using TrayGarden.Pipelines.Engine;
using TrayGarden.Plants.Pipeline;
using TrayGarden.Reception;
using TrayGarden.RuntimeSettings;

namespace TrayGarden.Plants;

public class Gardenbed : IGardenbed
{
  private readonly IRuntimeSettingsManager _runtimeSettingsManager;

  private readonly IPipelineRunner _pipelineRunner;


  public Gardenbed(IRuntimeSettingsManager runtimeSettingsManager, IPipelineRunner pipelineRunner)
  {
    _runtimeSettingsManager = runtimeSettingsManager;
    _pipelineRunner = pipelineRunner;
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

  private List<IPlant> GetAutoIncludePlants()
  {
    var result = new List<IPlant>();
    if (!AutoDetectPlants)
    {
      return result;
    }

    foreach (Assembly assembly in GetReferencedProjectAssemblies())
    {
      List<IPlant> plantsInAssembly = GetPlantsFromAssembly(assembly);
      if (plantsInAssembly != null && plantsInAssembly.Count > 0)
      {
        result.AddRange(plantsInAssembly);
      }
    }
    return result;
  }

  /// <summary>
  /// Plants are the project references of the host executable: its dependency manifest marks them as "project" libraries.
  /// </summary>
  private IEnumerable<Assembly> GetReferencedProjectAssemblies()
  {
    Assembly hostAssembly = Assembly.GetEntryAssembly();
    DependencyContext context = hostAssembly != null ? DependencyContext.Load(hostAssembly) : null;
    if (context == null)
    {
      Log.For(this).Warning("Gardenbed. Dependency manifest is not available, no plants will be discovered");
      yield break;
    }

    Assembly coreAssembly = typeof(IPlant).Assembly;
    foreach (RuntimeLibrary library in context.RuntimeLibraries.Where(x => x.Type == "project"))
    {
      Assembly assembly;
      try
      {
        assembly = Assembly.Load(new AssemblyName(library.Name));
      }
      catch (Exception ex)
      {
        Log.For(this).Error(ex, "Unable to load project assembly '{Assembly}'", library.Name);
        continue;
      }

      if (assembly != coreAssembly && assembly != hostAssembly)
      {
        yield return assembly;
      }
    }
  }

  private List<IPlant> GetPlantsFromAssembly(Assembly assembly)
  {
    try
    {
      var candidates = assembly.GetTypes().Where(x => !x.IsAbstract && !x.IsInterface && typeof(IPlant).IsAssignableFrom(x)).ToList();
      if (candidates.Count == 0)
      {
        return null;
      }
      Log.For(this).Information("We have found a suitable types in '{Assembly}' assembly", assembly.FullName);
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
      Log.For(this).Error(ex, "Unable to analyze assembly '{Assembly}' for plants", assembly.FullName);
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