using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Reception
{
  /// <summary>
  /// The base interface, which indicates that your object is a Plant. If declared object doesn't implement this interface, it justly will be ignored.
  /// </summary>
  public interface IPlant
  {
    string HumanSupportingName { get; }
    string Description { get; }
  }
}
