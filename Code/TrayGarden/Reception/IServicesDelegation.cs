#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Reception
{
  /// <summary>
  /// Allow to delegate service implementations to another objects. Otherwise the Plant object should implement service interfaces
  /// </summary>
  public interface IServicesDelegation
  {
    #region Public Methods and Operators

    List<object> GetServiceDelegates();

    #endregion
  }
}