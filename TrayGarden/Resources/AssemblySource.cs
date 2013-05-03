﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using TrayGarden.Configuration;
using TrayGarden.Helpers;

namespace TrayGarden.Resources
{
    public class AssemblySource : IRequireInitialization, ISource
    {
        public string ResourcePath { get; set; }
        public string AssemblyName { get; set; }

        public ResourceManager Source { get; protected set; }

        public virtual void Initialize()
        {
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