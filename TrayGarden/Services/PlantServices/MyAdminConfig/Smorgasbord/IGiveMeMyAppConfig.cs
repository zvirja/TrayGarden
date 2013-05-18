using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace TrayGarden.Services.PlantServices.MyAdminConfig.Smorgasbord
{
    public interface IGiveMeMyAppConfig
    {
        void SetModuleConfiguration(System.Configuration.Configuration moduleConfiguration);
    }
}
