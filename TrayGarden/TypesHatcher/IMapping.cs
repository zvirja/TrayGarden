using System;
using System.Xml;
using TrayGarden.Configuration;

namespace TrayGarden.TypesHatcher
{
    public interface IMapping
    {
        Type InterfaceType { get; }
        IObjectFactory ObjectFactory { get; }
    }
}