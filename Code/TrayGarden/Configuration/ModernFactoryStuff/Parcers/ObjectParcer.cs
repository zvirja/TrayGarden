using System.Xml;

namespace TrayGarden.Configuration.ModernFactoryStuff.Parcers;

public class ObjectParcer : IParcer
{
  public ObjectParcer(ModernFactory modernFactoryInstance)
  {
    this.ModernFactoryInstance = modernFactoryInstance;
  }

  protected ModernFactory ModernFactoryInstance { get; set; }

  public virtual object ParceNodeValue(XmlNode nodeValue)
  {
    var instance = this.ModernFactoryInstance.GetObject(nodeValue);
    return instance;
  }
}