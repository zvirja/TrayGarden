#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Configuration.ModernFactoryStuff.ContentAssigners
{
  public class NewListDirectAssigner : ListAssigner
  {
    #region Methods

    protected override System.Collections.IList GetListObject(System.Xml.XmlNode contentNode, object instance, System.Type instanceType)
    {
      return instance as IList;
    }

    #endregion
  }
}