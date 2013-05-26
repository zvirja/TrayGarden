using System.Collections.Generic;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces
{
    public interface IUserSettingsMetadataBuilder
    {
        void DeclareIntSetting(string settingName, int defaultValue);
        void DeclareBoolSetting(string settingName, bool defaultValue);
        void DeclareStringSetting(string settingName, string defaultValue);
        
        /// <summary>
        /// Allow user to select one of the possible options. User always has a way to select the default value.
        /// </summary>
        /// <param name="settingName"></param>
        /// <param name="options"></param>
        /// <param name="defaultValue"></param>
        void DeclareStringOptionSetting(string settingName, List<string> options, string defaultValue);

        /// <summary>
        /// Leave a way to extend the capabilies of the service. By default all the custom type settings are ignored
        /// </summary>
        /// <param name="type"></param>
        /// <param name="settingName"></param>
        /// <param name="defaultValue"></param>
        /// <param name="parameters"></param>
        void DeclareCustomTypeSetting(string type, string settingName, string defaultValue, object parameters);
        


    }
}