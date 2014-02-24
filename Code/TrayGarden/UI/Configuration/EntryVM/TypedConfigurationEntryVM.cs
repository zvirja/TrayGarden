#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.UI.Configuration.EntryVM.Players;

#endregion

namespace TrayGarden.UI.Configuration.EntryVM
{
  public abstract class TypedConfigurationEntryVM<TEntry> : ConfigurationEntryBaseVM
  {
    #region Constructors and Destructors

    public TypedConfigurationEntryVM([NotNull] ITypedConfigurationPlayer<TEntry> realPlayer)
      : base(realPlayer)
    {
      this.RealPlayer = realPlayer;
    }

    #endregion

    #region Public Properties

    public new ITypedConfigurationPlayer<TEntry> RealPlayer { get; set; }

    public TEntry Value
    {
      get
      {
        return this.RealPlayer.Value;
      }
      set
      {
        this.RealPlayer.Value = value;
        this.OnPropertyChanged("Value");
      }
    }

    #endregion

    #region Methods

    protected override void OnUnderlyingSettingValueChanged()
    {
      base.OnPropertyChanged("Value");
    }

    #endregion
  }
}