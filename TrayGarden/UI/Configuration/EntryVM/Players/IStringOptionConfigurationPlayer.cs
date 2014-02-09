#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.UI.Configuration.EntryVM.Players
{
  public interface IStringOptionConfigurationPlayer : ITypedConfigurationPlayer<string>
  {
    #region Public Properties

    List<string> Options { get; }

    #endregion
  }
}