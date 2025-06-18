using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.UI.Configuration.EntryVM.Players
{
  public interface IStringOptionConfigurationPlayer : ITypedConfigurationPlayer<string>
  {
    List<string> Options { get; }
  }
}