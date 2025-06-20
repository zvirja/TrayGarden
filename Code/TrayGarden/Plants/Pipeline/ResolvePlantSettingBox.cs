﻿using JetBrains.Annotations;

namespace TrayGarden.Plants.Pipeline;

[UsedImplicitly]
public class ResolvePlantSettingBox
{
  [UsedImplicitly]
  public virtual void Process(InitializePlantArgs args)
  {
    args.PlantSettingsBox = args.RootSettingsBox.GetSubBox(args.PlantID);
  }
}