﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Services.PlantServices.MyAdminConfig.Smorgasbord;

/// <summary>
/// This service provides plant with appropriate App.config file content (for appropriate assembly).
/// </summary>
public interface IGiveMeMyAppConfig
{
  void StoreModuleConfiguration(System.Configuration.Configuration moduleConfiguration);
}