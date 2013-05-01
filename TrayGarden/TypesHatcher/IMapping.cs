using System;
using System.Xml;

namespace TrayGarden.TypesHatcher
{
    public interface IMapping
    {
        bool IsSingletone { get; }
        Type InterfaceType { get; }
        string InstanceConfigurationPath { get; }
    }
}