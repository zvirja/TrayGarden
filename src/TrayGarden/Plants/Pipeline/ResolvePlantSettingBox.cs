using JetBrains.Annotations;

using TrayGarden.Pipelines.Engine;

namespace TrayGarden.Plants.Pipeline;

[UsedImplicitly]
public class ResolvePlantSettingBox : IPipelineProcessor<InitializePlantArgs>
{
  [UsedImplicitly]
  public void Process(InitializePlantArgs args)
  {
    args.PlantSettingsBox = args.RootSettingsBox.GetSubBox(args.PlantID);
  }
}
