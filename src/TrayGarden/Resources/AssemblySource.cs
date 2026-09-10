using System;
using System.Linq;
using System.Reflection;
using System.Resources;
using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;

namespace TrayGarden.Resources;

[UsedImplicitly]
public class AssemblySource : ISource
{
  public ResourceManager Source { get; private set; }

  private string AssemblyName { get; set; }

  private string ResourcePath { get; set; }

  [UsedImplicitly]
  public void Initialize([NotNull] string assemblyName, [NotNull] string resourcePath)
  {
    Assert.ArgumentNotNullOrEmpty(assemblyName, "assemblyName");
    Assert.ArgumentNotNullOrEmpty(resourcePath, "resourcePath");
    AssemblyName = assemblyName;
    ResourcePath = resourcePath;
    var assembly = ResolveAssembly(AssemblyName);
    if (assembly != null)
    {
      Source = new ResourceManager(ResourcePath, assembly);
    }
  }

  private Assembly ResolveAssembly(string assemblyName)
  {
    if (assemblyName.IsNullOrEmpty())
    {
      return null;
    }
    var assembly =
      AppDomain.CurrentDomain.GetAssemblies()
        .FirstOrDefault(x => x.GetName().Name.Equals(assemblyName, StringComparison.OrdinalIgnoreCase));
    if (assembly == null)
    {
      try
      {
        assembly = AppDomain.CurrentDomain.Load(assemblyName);
      }
      catch (Exception ex)
      {
        Log.For(this).Warning(ex, "Can't load assembly {AssemblyName}", assemblyName);
      }
    }
    return assembly;
  }
}