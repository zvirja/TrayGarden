﻿using System.Collections.Generic;

namespace TrayGarden.Reception;

/// <summary>
/// Allow to delegate service implementations to another objects. Otherwise the Plant object should implement service interfaces
/// </summary>
public interface IServicesDelegation
{
  List<object> GetServiceDelegates();
}