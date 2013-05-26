using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Services.PlantServices.UserConfig.Core
{
    public interface IUserSettingMaster:IUserSetting
    {
        UserSettingValueType ValueType { get; }

        bool Initialize(UserSettingValueType valueType, object defaultValue, object additionalInfo);

        bool SetIntValue(int value);
        bool SetBoolValue(bool value);
        bool SetStringValue(string value);
        bool SetStringOptionValue(string value);

        /// <summary>
        /// Leave a way to extend service. By default this value always returns false.
        /// </summary>
        bool SetCustomTypeValue(string value);

        
    }
}
