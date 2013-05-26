using System;
using System.Collections.Generic;
using System.Globalization;
using JetBrains.Annotations;
using TrayGarden.Configuration;
using TrayGarden.Helpers;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;
using System.Linq;
using TrayGarden.Services.PlantServices.UserConfig.Core.Stuff;

namespace TrayGarden.Services.PlantServices.UserConfig.Core
{
    public class UserSettingsMetadataBuilder : IUserSettingsMetadataBuilderMaster
    {
        protected IObjectFactory MetadataInstanceFactory { get; set; }
        protected bool Initialized { get; set; }
        protected Dictionary<string, IUserSettingMetadataMaster> SettingsMetadata { get; set; }

        public UserSettingsMetadataBuilder()
        {
            Initialized = false;
            SettingsMetadata = new Dictionary<string, IUserSettingMetadataMaster>();
        }

        [UsedImplicitly]
        public virtual void Initialize([NotNull] IObjectFactory metadataInstanceFactory)
        {
            if (metadataInstanceFactory == null) throw new ArgumentNullException("metadataInstanceFactory");
            MetadataInstanceFactory = metadataInstanceFactory;
            Initialized = true;
        }

        public virtual void DeclareIntSetting([NotNull] string settingName, int defaultValue)
        {
            AssertInitialized();
            if (settingName.IsNullOrEmpty()) throw new ArgumentException("settingName");
            DeclareSettingMetadata(settingName, UserSettingValueType.Int, defaultValue.ToString(CultureInfo.InvariantCulture), null);
        }

        public virtual void DeclareBoolSetting(string settingName, bool defaultValue)
        {
            AssertInitialized();
            if (settingName.IsNullOrEmpty()) throw new ArgumentException("settingName");
            DeclareSettingMetadata(settingName, UserSettingValueType.Bool, defaultValue.ToString(CultureInfo.InvariantCulture), null);

        }

        public virtual void DeclareStringSetting(string settingName, string defaultValue)
        {
            AssertInitialized();
            if (settingName.IsNullOrEmpty()) throw new ArgumentException("settingName");
            DeclareSettingMetadata(settingName, UserSettingValueType.String, defaultValue, null);
        }

        public virtual void DeclareStringOptionSetting(string settingName, [NotNull] List<string> options, string defaultValue)
        {
            AssertInitialized();
            if (settingName.IsNullOrEmpty()) throw new ArgumentException("settingName");
            if (options == null || options.Count == 0) throw new ArgumentException("options");
            DeclareSettingMetadata(settingName, UserSettingValueType.StringOption, defaultValue.ToString(CultureInfo.InvariantCulture), options);
        }

        public virtual void DeclareCustomTypeSetting(string type, string settingName, string defaultValue, object parameters)
        {
            AssertInitialized();
            if (type.IsNullOrEmpty()) throw new ArgumentException("type");
            if (settingName.IsNullOrEmpty()) throw new ArgumentException("settingName");
            DeclareSettingMetadata(settingName, UserSettingValueType.StringOption, defaultValue.ToString(CultureInfo.InvariantCulture), new[] { type, parameters });
        }

        public virtual List<IUserSettingMetadataMaster> GetResultingSettingsMetadata()
        {
            return this.SettingsMetadata.Select(x => x.Value).ToList();
        }

        protected virtual void AssertInitialized()
        {
            if (!Initialized)
                throw new NonInitializedException();
        }

        protected virtual void DeclareSettingMetadata(string name, UserSettingValueType valueType, string defaultValue, object additionalParams)
        {
            if (this.SettingsMetadata.ContainsKey(name))
                throw new ArgumentException("The setting with same name already exists");
            var settingMetadata = MetadataInstanceFactory.GetPurelyNewObject() as IUserSettingMetadataMaster;
            if (settingMetadata == null)
                return;
            settingMetadata.Initialize(name, valueType, defaultValue, additionalParams);
            SettingsMetadata.Add(name, settingMetadata);
        }
    }
}