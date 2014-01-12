using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

namespace TrayGarden.UI.Configuration.Stuff
{
  public class ConfigurationEntryForDoubleVM : ConfigurationEntryVMBase
  {
    #region Constructors and Destructors

    public ConfigurationEntryForDoubleVM([NotNull] IConfigurationAwarePlayerWithValues realPlayer)
      : base(realPlayer)
    {
    }

    #endregion

    #region Public Properties

    public double DoubleValue
    {
      get
      {
        return this.GetSpecificRealPlayer<IConfigurationAwarePlayerWithValues>().DoubleValue;
      }
      set
      {
        if (value == this.GetSpecificRealPlayer<IConfigurationAwarePlayerWithValues>().DoubleValue)
        {
          return;
        }
        this.GetSpecificRealPlayer<IConfigurationAwarePlayerWithValues>().DoubleValue = value;
        this.OnPropertyChanged("DoubleValue");
      }
    }

    #endregion

    #region Methods

    protected override void OnUnderlyingSettingValueChanged()
    {
      base.OnUnderlyingSettingValueChanged();
      this.OnPropertyChanged("DoubleValue");
    }

    #endregion
  }
}