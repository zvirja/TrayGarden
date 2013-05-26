using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Services.PlantServices.UserConfig.Core
{
    public interface IUserSetting
    {
        int? IntValue { get; set; }
        bool? BoolValue { get; set; }
        string StringValue { get; set; }
        string StringOptionValue { get; set; }



        /// <summary>
        /// Leave a way to extend service. By default this value always is null.
        /// </summary>
        string CustomTypeValue { get; set; }

        UserSettingValueType ValueType { get; }
        IUserSettingMetadata Metadata { get; }
    }
}
