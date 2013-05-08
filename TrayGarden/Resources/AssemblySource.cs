using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Configuration;
using TrayGarden.Helpers;

namespace TrayGarden.Resources
{
    [UsedImplicitly]
    public class AssemblySource : ISource
    {
        protected string ResourcePath { get; set; }
        protected string AssemblyName { get; set; }

        public ResourceManager Source { get; protected set; }

        [UsedImplicitly]
        public virtual void Initialize(string assemblyName, string resourcePath)
        {
            AssemblyName = assemblyName;
            ResourcePath = resourcePath;
            var assembly = ResolveAssembly(AssemblyName);
            if (assembly != null)
                Source = new ResourceManager(ResourcePath, assembly);
        }

        protected virtual Assembly ResolveAssembly(string assemblyName)
        {
            if (assemblyName.IsNullOrEmpty())
                return null;
            var assembly =
                AppDomain.CurrentDomain.GetAssemblies()
                         .FirstOrDefault(
                             x => x.GetName().Name.Equals(assemblyName, StringComparison.OrdinalIgnoreCase));
            if (assembly == null)
            {
                try
                {
                    assembly = AppDomain.CurrentDomain.Load(assemblyName);
                }
                catch
                {
                    return null;
                }
            }
            return assembly;
        }
    }
}