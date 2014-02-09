#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.UI.Configuration.EntryVM.Players
{
  public interface ITypedConfigurationPlayer<TEntry> : IConfigurationPlayer
  {
    #region Public Properties

    TEntry Value { get; set; }

    #endregion
  }
}