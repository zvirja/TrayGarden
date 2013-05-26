using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Services.PlantServices.UserConfig.Core
{
    public interface IUserSettingMetadata
    {
        string Name { get;  }
        string DefaultValue { get;  }
        UserSettingValueType SettingValueType { get;  }
        object AdditionalParams { get; }
    }
}
