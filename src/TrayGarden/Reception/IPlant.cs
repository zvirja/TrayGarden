namespace TrayGarden.Reception;

/// <summary>
/// The base interface, which indicates that your object is a Plant. If declared object doesn't implement this interface, it justly will be ignored.
/// </summary>
public interface IPlant
{
  string Description { get; }

  string HumanSupportingName { get; }

  void Initialize();

  void PostServicesInitialize();
}