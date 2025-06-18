using System.Resources;

namespace TrayGarden.Resources;

public interface ISource
{
  ResourceManager Source { get; }
}