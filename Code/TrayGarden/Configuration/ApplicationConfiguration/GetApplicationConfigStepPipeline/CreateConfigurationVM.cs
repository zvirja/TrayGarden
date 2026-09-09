using JetBrains.Annotations;

using TrayGarden.Pipelines.Engine;
using TrayGarden.UI.ForSimplerLife;

namespace TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline;

[UsedImplicitly]
public class CreateConfigurationVM : IPipelineProcessor<GetApplicationConfigStepArgs>
{
  [UsedImplicitly]
  public void Process(GetApplicationConfigStepArgs args)
  {
    ConfigurationControlConstructInfo configurationInfo = args.ConfigurationConstructInfo;
    configurationInfo.BuildControlVM();
    args.StepConstructInfo.ContentVM = configurationInfo.ResultControlVM;
  }
}
