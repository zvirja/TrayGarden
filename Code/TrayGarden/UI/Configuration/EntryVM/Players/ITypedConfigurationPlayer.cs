namespace TrayGarden.UI.Configuration.EntryVM.Players;

public interface ITypedConfigurationPlayer<TEntry> : IConfigurationPlayer
{
  TEntry Value { get; set; }
}