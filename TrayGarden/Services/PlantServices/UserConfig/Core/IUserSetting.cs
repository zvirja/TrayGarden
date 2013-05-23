using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Services.PlantServices.UserConfig.Core
{
    public interface IUserSetting
    {
        int? IntValue { get; }
        bool? BoolValue { get; }
        string StringValue { get; }
        string StringOptionValue { get; }

        /// <summary>
        /// Leave a way to extend service. By default this value always is null.
        /// </summary>
        string CustomTypeValue { get; }

    }
}
