using JetBrains.Annotations;

using TrayGarden.UI.ForSimplerLife;

namespace TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline;

[UsedImplicitly]
public class CreateConfigurationVM
{
  [UsedImplicitly]
  public virtual void Process(GetApplicationConfigStepArgs args)
  {
    ConfigurationControlConstructInfo configurationInfo = args.ConfigurationConstructInfo;
    configurationInfo.BuildControlVM();
    args.StepConstructInfo.ContentVM = configurationInfo.ResultControlVM;
  }
}