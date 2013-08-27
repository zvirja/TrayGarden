using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ClipboardChangerPlant.RequestHandling.Specialized
{
  public class Clip2NetWithoutRequestHandler : RequestHandler
  {
    public override bool TryProcess(string inputValue, out string result)
    {
      result = null;
      try
      {
        result = ResolveAsXml(inputValue);
        return true;
      }
      catch
      {
        return false;
      }
    }

    protected virtual string ResolveAsXml(string inputValue)
    {
      var root = XElement.Parse(inputValue);
      var imgNode = root.Element("img");
      return imgNode.Attribute("src").Value;
    }
  }
}
