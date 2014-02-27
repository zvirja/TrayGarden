#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

#endregion

namespace TrayGarden.Configuration
{
  public class SectionHandler : System.Configuration.ConfigurationSection
  {
    #region Public Properties

    public XmlDocument XmlRepresentation { get; set; }

    #endregion

    #region Methods

    protected override void DeserializeSection(XmlReader reader)
    {
      var document = new XmlDocument();
      document.Load(reader);
      this.XmlRepresentation = document;
    }

    #endregion
  }
}