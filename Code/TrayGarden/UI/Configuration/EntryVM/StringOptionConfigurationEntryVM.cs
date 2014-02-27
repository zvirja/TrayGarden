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
  public class StringOptionConfigurationEntryVM : TypedConfigurationEntryVM<string>
  {
    #region Constructors and Destructors

    public StringOptionConfigurationEntryVM([NotNull] IStringOptionConfigurationPlayer realPlayer)
      : base(realPlayer)
    {
      this.RealPlayer = realPlayer;
    }

    #endregion

    #region Public Properties

    public List<string> AllPossibleOptions
    {
      get
      {
        return this.GetAllPossibleOptions();
      }
    }

    #endregion

    #region Properties

    protected new IStringOptionConfigurationPlayer RealPlayer { get; set; }

    #endregion

    #region Methods

    protected virtual List<string> GetAllPossibleOptions()
    {
      return this.RealPlayer.Options;
    }

    #endregion
  }
}