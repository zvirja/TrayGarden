using System;
using System.Collections.Generic;
using System.Globalization;
using JetBrains.Annotations;
using TrayGarden.Configuration;
using TrayGarden.Diagnostics;
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
            Assert.ArgumentNotNull(metadataInstanceFactory, "metadataInstanceFactory");
            MetadataInstanceFactory = metadataInstanceFactory;
            Initialized = true;
        }

        public virtual void DeclareIntSetting([NotNull] string settingName, int defaultValue)
        {
            Assert.ArgumentNotNullOrEmpty(settingName, "settingName");
            AssertInitialized();
            if (settingName.IsNullOrEmpty()) throw new ArgumentException("settingName");
            DeclareSettingMetadata(settingName, UserSettingValueType.Int, defaultValue.ToString(CultureInfo.InvariantCulture), null);
        }

        public virtual void DeclareBoolSetting([NotNull] string settingName, bool defaultValue)
        {
            Assert.ArgumentNotNullOrEmpty(settingName, "settingName");
            AssertInitialized();
            if (settingName.IsNullOrEmpty()) throw new ArgumentException("settingName");
            DeclareSettingMetadata(settingName, UserSettingValueType.Bool, defaultValue.ToString(CultureInfo.InvariantCulture), null);

        }

        public virtual void DeclareStringSetting(string settingName, string defaultValue)
        {
            Assert.ArgumentNotNullOrEmpty(settingName, "settingName");
            AssertInitialized();
            if (settingName.IsNullOrEmpty()) throw new ArgumentException("settingName");
            DeclareSettingMetadata(settingName, UserSettingValueType.String, defaultValue, null);
        }

        public virtual void DeclareStringOptionSetting(string settingName, [NotNull] List<string> options, string defaultValue)
        {
            Assert.ArgumentNotNullOrEmpty(settingName, "settingName");
            AssertInitialized();
            if (options == null || options.Count == 0) throw new ArgumentException("options");
            DeclareSettingMetadata(settingName, UserSettingValueType.StringOption, defaultValue.ToString(CultureInfo.InvariantCulture), options);
        }

        public virtual void DeclareCustomTypeSetting([NotNull] string type, [NotNull] string settingName, string defaultValue, object parameters)
        {
            Assert.ArgumentNotNullOrEmpty(settingName, "settingName");
            Assert.ArgumentNotNullOrEmpty(type, "type");
            AssertInitialized();
            DeclareSettingMetadata(settingName, UserSettingValueType.CustomType, defaultValue.ToString(CultureInfo.InvariantCulture), new[] { type, parameters });
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
            Assert.IsNotNull(settingMetadata,"Metadata instance factory returns wrong objects");
            settingMetadata.Initialize(name, valueType, defaultValue, additionalParams);
            SettingsMetadata.Add(name, settingMetadata);
        }

        
    }
}