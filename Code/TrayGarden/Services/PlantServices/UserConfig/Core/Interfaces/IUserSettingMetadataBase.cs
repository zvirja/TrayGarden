using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces
{
  public interface IUserSettingMetadataBase
  {
    object AdditionalParams { get; }

    string Description { get; }

    IUserSettingHallmark Hallmark { get; }

    string Name { get; }

    string Title { get; }
  }
}