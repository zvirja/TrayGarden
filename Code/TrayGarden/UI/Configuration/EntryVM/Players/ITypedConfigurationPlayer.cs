using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.UI.Configuration.EntryVM.Players
{
  public interface ITypedConfigurationPlayer<TEntry> : IConfigurationPlayer
  {
    TEntry Value { get; set; }
  }
}