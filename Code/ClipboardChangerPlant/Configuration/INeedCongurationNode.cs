#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

#endregion

namespace ClipboardChangerPlant.Configuration
{
  public interface INeedCongurationNode
  {
    #region Public Properties

    string Name { get; set; }

    #endregion

    #region Public Methods and Operators

    void SetConfigurationNode(XmlNode configurationNode);

    #endregion
  }
}