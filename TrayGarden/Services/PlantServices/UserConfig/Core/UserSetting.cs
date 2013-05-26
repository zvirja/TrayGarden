using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using TrayGarden.RuntimeSettings;
using TrayGarden.Services.PlantServices.UserConfig.Core.Stuff;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces
{
    public class UserSetting : IUserSettingMaster
    {
        protected int? _intValue;
        protected bool? _boolValue;
        protected string _stringValue;
        protected string _stringOptionValue;
        protected IUserSettingMetadata _metadata;

        protected List<string> StringOptions { get; set; }

        protected bool Initialized { get; set; }
        protected ISettingsBox SettingsBox { get; set; }

        public event UserSettingValueChanged Changed;

       public virtual UserSettingValueType ValueType
        {
            get
            {
                AssertInitialized();
                return _metadata.SettingValueType;
            }
        }

        public virtual string Name
        {
            get { return _metadata.Name; }
        }

        public virtual int? IntValue
        {
            get
            {
                AssertInitialized();
                return _intValue;
            }
            set
            {
                AssertInitialized();
                if (ValueType != UserSettingValueType.Int)
                    HandleWrongTypeAssigning();
                var beforeState = GetSnapshot();
                _intValue = value;
                StoreValueInSettingsStorage(value.ToString());
                OnChanged(beforeState,this);
            }
        }

        public virtual bool? BoolValue
        {
            get
            {
                AssertInitialized();
                return _boolValue;
            }
            set
            {
                AssertInitialized();
                if (ValueType != UserSettingValueType.Bool)
                    HandleWrongTypeAssigning();
                var beforeState = GetSnapshot();
                _boolValue = value;
                StoreValueInSettingsStorage(value.ToString());
                OnChanged(beforeState, this);
            }
        }

        public virtual string StringValue
        {
            get
            {
                AssertInitialized();
                return _stringValue;
            }
            set
            {
                AssertInitialized();
                if (ValueType != UserSettingValueType.String)
                    HandleWrongTypeAssigning();
                var beforeState = GetSnapshot();
                _stringValue = value;
                StoreValueInSettingsStorage(value);
                OnChanged(beforeState, this);
            }
        }

        public virtual string StringOptionValue
        {
            get
            {
                AssertInitialized();
                return _stringOptionValue;
            }
            set
            {
                AssertInitialized();
                if (ValueType != UserSettingValueType.StringOption)
                    HandleWrongTypeAssigning();
                var beforeState = GetSnapshot();
                if (ValidateValueForStringOption(value, StringOptions))
                    _stringOptionValue = value;
                else
                    _stringOptionValue = Metadata.DefaultValue;
                StoreValueInSettingsStorage(_stringOptionValue);
                OnChanged(beforeState, this);
            }
        }

        public virtual string CustomTypeValue
        {
            get { return null; }
            set { }
        }



        public virtual IUserSettingMetadata Metadata
        {
            get
            {
                AssertInitialized();
                return _metadata;
            }
            protected set { _metadata = value; }

        }

        public UserSetting()
        {
            Initialized = false;
        }


        public virtual void Initialize([NotNull] IUserSettingMetadata metadata,
                                       [NotNull] ISettingsBox containerSettingsBox)
        {
            if (metadata == null) throw new ArgumentNullException("metadata");
            if (containerSettingsBox == null) throw new ArgumentNullException("contaiterSettingsBox");
            Metadata = metadata;
            SettingsBox = containerSettingsBox;
            SetInitialSettingValue(metadata, containerSettingsBox);
            Initialized = true;
        }


        protected virtual void AssertInitialized()
        {
            if (!Initialized)
                throw new NonInitializedException();
        }

        protected virtual void HandleWrongTypeAssigning()
        {
            throw new ArgumentException("The setting type is different");
        }

        protected virtual void SetInitialSettingValue(IUserSettingMetadata metadata, ISettingsBox settingsBox)
        {
            UserSettingValueType valueType = metadata.SettingValueType;
            if (valueType == UserSettingValueType.Int)
            {
                int newValue;
                if (settingsBox.TryGetInt(metadata.Name, out newValue))
                {
                    _intValue = newValue;
                    return;
                }
                if (int.TryParse(metadata.DefaultValue, out newValue))
                {
                    _intValue = newValue;
                    return;
                }
                _intValue = 0;
                return;
            }
            if (valueType == UserSettingValueType.Bool)
            {
                bool newValue;
                if (settingsBox.TryGetBool(metadata.Name, out newValue))
                {
                    _boolValue = newValue;
                    return;
                }
                if (bool.TryParse(metadata.DefaultValue, out newValue))
                {
                    _boolValue = newValue;
                    return;
                }
                _boolValue = false;
                return;
            }
            if (valueType == UserSettingValueType.String)
            {
                string newValue = settingsBox.GetString(metadata.Name, null);
                if (newValue != null)
                {
                    _stringValue = newValue;
                    return;
                }
                _stringValue = metadata.DefaultValue;
                return;
            }
            if (valueType == UserSettingValueType.StringOption)
            {
                var possibleValues = metadata.AdditionalParams as List<string>;
                possibleValues = possibleValues ?? new List<string>();
                StringOptions = possibleValues;
                string newValue = settingsBox.GetString(metadata.Name, null);
                if (newValue != null && ValidateValueForStringOption(newValue, possibleValues))
                {
                    _stringOptionValue = newValue;
                    return;
                }
                _stringOptionValue = metadata.DefaultValue;
            }


        }

        protected virtual bool ValidateValueForStringOption(string value, List<string> possibleOptions)
        {
            return possibleOptions.Contains(value);
        }

        protected virtual void StoreValueInSettingsStorage(string value)
        {
            SettingsBox.SetString(Name,value);
        }

        protected virtual void OnChanged(UserSetting statebefore, UserSetting stateafter)
        {
            UserSettingValueChanged handler = Changed;
            if (handler != null) handler(statebefore, stateafter);
        }

        protected virtual UserSetting GetSnapshot()
        {
            var clone = (UserSetting)MemberwiseClone();
            return clone;
        }

    }
}