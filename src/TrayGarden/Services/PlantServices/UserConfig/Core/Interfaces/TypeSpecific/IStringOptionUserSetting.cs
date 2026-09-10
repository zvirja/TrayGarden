using System.Collections.Generic;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces.TypeSpecific;

public interface IStringOptionUserSetting : ITypedUserSetting<string>
{
  List<string> PossibleOptions { get; }
}