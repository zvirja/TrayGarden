using System.Collections.Generic;

namespace TrayGarden.UI.Configuration.EntryVM.Players;

public interface IStringOptionConfigurationPlayer : ITypedConfigurationPlayer<string>
{
  List<string> Options { get; }
}