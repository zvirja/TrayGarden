﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.RuntimeSettings;

namespace TrayGarden.Services.PlantServices.CustomSettings.Smorgasbord
{
    public interface IStoreCustomSettingsStorage
    {
        void StoreCustomSettingsStorage(ISettingsBox settingsStorage);
    }
}
