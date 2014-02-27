#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Reception
{
  /// <summary>
  /// The base interface, which indicates that your object is a Plant. If declared object doesn't implement this interface, it justly will be ignored.
  /// </summary>
  public interface IPlant
  {
    #region Public Properties

    string Description { get; }

    string HumanSupportingName { get; }

    #endregion

    #region Public Methods and Operators

    void Initialize();

    void PostServicesInitialize();

    #endregion
  }
}