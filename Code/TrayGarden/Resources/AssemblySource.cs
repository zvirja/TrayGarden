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
  public ResourceManager Source { get; protected set; }

  protected string AssemblyName { get; set; }

  protected string ResourcePath { get; set; }

  [UsedImplicitly]
  public virtual void Initialize([NotNull] string assemblyName, [NotNull] string resourcePath)
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

  protected virtual Assembly ResolveAssembly(string assemblyName)
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
        Log.Warn("Can't load assembly {0}".FormatWith(assemblyName), this, ex);
      }
    }
    return assembly;
  }
}