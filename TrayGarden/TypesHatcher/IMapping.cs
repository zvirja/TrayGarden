using System;
using System.Xml;

namespace TrayGarden.TypesHatcher
{
    public interface IMapping
    {
        bool IsSingleton { get; }
        Type InterfaceType { get; }
        string InstanceConfigurationPath { get; }
    }
}