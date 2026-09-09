using Microsoft.Extensions.DependencyInjection;

using TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline;
using TrayGarden.Configuration.ApplicationConfiguration.Injection;
using TrayGarden.DummyTests;
using TrayGarden.Pipelines.RestartApp;
using TrayGarden.Pipelines.Shutdown;
using TrayGarden.Pipelines.Startup;
using TrayGarden.Plants.Intergration;
using TrayGarden.Plants.Pipeline;
using TrayGarden.Services.Engine.UI.GetStateForServicesConfigurationPipeline;
using TrayGarden.Services.Engine.UI.Intergration;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.InitPlantPipeline;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ResolveSinglePlantVMPipeline;
using TrayGarden.Services.PlantServices.RareCommands.Pipelines.PlantInit;
using TrayGarden.Services.PlantServices.RareCommands.UI;
using TrayGarden.Services.PlantServices.StandaloneIcon.Core.InitPlantPipeline;
using TrayGarden.Services.PlantServices.UserConfig.Pipelines.GetWindowStep;
using TrayGarden.Services.PlantServices.UserConfig.Pipelines.PlantInit;
using TrayGarden.Services.PlantServices.UserConfig.UI.Intergration;
using TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration.UIInteraction.GetStepPipeline;
using TrayGarden.Services.PlantServices.UserNotifications.Core.Integration;
using TrayGarden.UI.MainWindow.ResolveVMPipeline;
using TrayGarden.UI.WindowWithReturn.Integration;

using AppConfigStep = TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline;
using ServicesConfigStep = TrayGarden.Services.Engine.UI.GetStateForServicesConfigurationPipeline;
using UserConfigWindowStep = TrayGarden.Services.PlantServices.UserConfig.Pipelines.GetWindowStep;
using UserNotificationsConfigStep =
  TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration.UIInteraction.GetStepPipeline;

namespace TrayGarden.Composition;

/// <summary>
/// In-memory replacement for the pipeline definitions that used to live in
/// App.config. Processor order within each pipeline is the registration order.
/// Three step-building processor names recur across the configuration pipelines
/// (ResolveConfigurationEntries, CreateConfigurationVM, CreateStep); those four
/// namespaces get an alias so every processor is still named plainly below.
/// </summary>
public static class Pipelines
{
  public static IServiceCollection AddGardenPipelines(this IServiceCollection services)
  {
    services.AddPipeline<StartupArgs>()
      .Add<SingleInstanceCheckAndHooks>()
      .Add<PlantServicesInformInit>()
      .Add<GardenbedInformPostServicesInit>()
      .Add<PlantServicesInformDisplay>()
      .Add<OpenConfigDiaglogIfNeed>();

    services.AddPipeline<ShutdownArgs>()
      .Add<PlantServicesInformClose>();

    services.AddPipeline<RestartAppArgs>()
      .Add<StopSingleInstanceMonitor>()
      .Add<SimpleAppRestart>();

    services.AddPipeline<InitializePlantArgs>()
      .Add<ResolveIPlant>()
      .Add<ResolvePlantID>()
      .Add<InitializePlant>()
      .Add<ValidatePlant>()
      .Add<ResolveWorkhorses>()
      .Add<ResolvePlantSettingBox>()
      .Add<CreateIPlantEx>();

    services.AddPipeline<InitPlantSIArgs>()
      .Add<CreateSIPlantBox>()
      .Add<CreateNotifyIcon>()
      .Add<BuildContextMenu>()
      .Add<AssignIconModifier>()
      .Add<ValidateAndAssignSIBox>()
      .Add<ResolveSettingsBox>();

    services.AddPipeline<InitPlantGMArgs>()
      .Add<CreatePlantBox>()
      .Add<CreateSettingsBox>()
      .Add<CreateContextMenuStrip>()
      .Add<ProvideWithIconChanger>()
      .Add<BindPlantBoxToPlant>();

    services.AddPipeline<InitPlantUCPipelineArg>()
      .Add<ResolveWorkhorse>()
      .Add<ResolveSettingBox>()
      .Add<CreatePersonalSettingsSteward>()
      .Add<ProvidePlantWithSteward>()
      .Add<AssignPlantBox>();

    services.AddPipeline<InitPlantRareCommandsArgs>()
      .Add<CollectRareCommands>();

    services.AddPipeline<GetMainVMPipelineArgs>()
      .Add<CreateViewModel>()
      .Add<ResolvePlantsConfigVM>()
      .Add<InjectApplicationConfigLink>()
      .Add<InitializeFirstStep>();

    services.AddPipeline<ResolveSinglePlantVMPipelineArgs>()
      .Add<CreatePlantVM>()
      .Add<StandaloneIconServicePresenter>()
      .Add<GlobalMenuServiceMenuEmbeddingPresenter>()
      .Add<GlobalMenuServiceIconChangePresenter>()
      .Add<ClipboardListenerPresenter>()
      .Add<UserNotificationsPresenter>()
      .Add<RareCommandsPresenter>()
      .Add<UserConfigPresenter>();

    services.AddPipeline<GetUCStepPipelineArgs>()
      .Add<UserConfigWindowStep.ResolveConfigurationEntries>()
      .Add<ResolveContentVM>()
      .Add<AddResetAllHelpAction>()
      .Add<CreateStepInfo>();

    services.AddPipeline<GetStateForServicesConfigurationPipelineArgs>()
      .Add<InitializeGeneralSettings>()
      .Add<ServicesConfigStep.ResolveConfigurationEntries>()
      .Add<PlantServiceConfigurator>()
      .Add<ServicesConfigStep.CreateConfigurationVM>()
      .Add<MakeResetAllCommandVisible>()
      .Add<CreateWindowWithBackState>();

    services.AddPipeline<GetApplicationConfigStepArgs>()
      .Add<AssignVisibleText>()
      .Add<AddRunAtStartupSetting>()
      .Add<AppConfigStep.CreateConfigurationVM>()
      .Add<ServicesConfigurationInjectSAInjector>()
      .Add<AutoLoadAssembliesSetting>()
      .Add<ExitOnCloseSetting>()
      .Add<DummySettingInjection>()
      .Add<AppConfigStep.CreateStep>();

    services.AddPipeline<UNConfigurationStepArgs>()
      .Add<TuneConfigurationProperties>()
      .Add<TuneWindowProperties>()
      .Add<AddConfigurationEntries>()
      .Add<CreateConfigurationControl>()
      .Add<UserNotificationsConfigStep.CreateStep>();

    return services;
  }
}
