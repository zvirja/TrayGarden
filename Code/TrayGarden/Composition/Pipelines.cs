using Microsoft.Extensions.DependencyInjection;

using AppConfig = TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline;
using GmInit = TrayGarden.Services.PlantServices.GlobalMenu.Core.InitPlantPipeline;
using MainVm = TrayGarden.UI.MainWindow.ResolveVMPipeline;
using PlantInit = TrayGarden.Plants.Pipeline;
using RareInit = TrayGarden.Services.PlantServices.RareCommands.Pipelines.PlantInit;
using Restart = TrayGarden.Pipelines.RestartApp;
using ServicesCfg = TrayGarden.Services.Engine.UI.GetStateForServicesConfigurationPipeline;
using Shutdown = TrayGarden.Pipelines.Shutdown;
using SiInit = TrayGarden.Services.PlantServices.StandaloneIcon.Core.InitPlantPipeline;
using SinglePlant = TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ResolveSinglePlantVMPipeline;
using Startup = TrayGarden.Pipelines.Startup;
using UcStep = TrayGarden.Services.PlantServices.UserConfig.Pipelines.GetWindowStep;
using UcInit = TrayGarden.Services.PlantServices.UserConfig.Pipelines.PlantInit;
using UnStep = TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration.UIInteraction.GetStepPipeline;

namespace TrayGarden.Composition;

/// <summary>
/// In-memory replacement for the pipeline definitions that used to live in
/// App.config. Processor order within each pipeline is the registration order.
/// </summary>
public static class Pipelines
{
  public static IServiceCollection AddGardenPipelines(this IServiceCollection services)
  {
    services.AddPipeline<Startup.StartupArgs>()
      .Add<Startup.SingleInstanceCheckAndHooks>()
      .Add<Startup.PlantServicesInformInit>()
      .Add<Startup.GardenbedInformPostServicesInit>()
      .Add<Startup.PlantServicesInformDisplay>()
      .Add<Startup.OpenConfigDiaglogIfNeed>();

    services.AddPipeline<Shutdown.ShutdownArgs>()
      .Add<Shutdown.PlantServicesInformClose>();

    services.AddPipeline<Restart.RestartAppArgs>()
      .Add<Restart.StopSingleInstanceMonitor>()
      .Add<Restart.SimpleAppRestart>();

    services.AddPipeline<PlantInit.InitializePlantArgs>()
      .Add<PlantInit.ResolveIPlant>()
      .Add<PlantInit.ResolvePlantID>()
      .Add<PlantInit.InitializePlant>()
      .Add<PlantInit.ValidatePlant>()
      .Add<PlantInit.ResolveWorkhorses>()
      .Add<PlantInit.ResolvePlantSettingBox>()
      .Add<PlantInit.CreateIPlantEx>();

    services.AddPipeline<SiInit.InitPlantSIArgs>()
      .Add<SiInit.CreateSIPlantBox>()
      .Add<SiInit.CreateNotifyIcon>()
      .Add<SiInit.BuildContextMenu>()
      .Add<SiInit.AssignIconModifier>()
      .Add<SiInit.ValidateAndAssignSIBox>()
      .Add<SiInit.ResolveSettingsBox>();

    services.AddPipeline<GmInit.InitPlantGMArgs>()
      .Add<GmInit.CreatePlantBox>()
      .Add<GmInit.CreateSettingsBox>()
      .Add<GmInit.CreateContextMenuStrip>()
      .Add<GmInit.ProvideWithIconChanger>()
      .Add<GmInit.BindPlantBoxToPlant>();

    services.AddPipeline<UcInit.InitPlantUCPipelineArg>()
      .Add<UcInit.ResolveWorkhorse>()
      .Add<UcInit.ResolveSettingBox>()
      .Add<UcInit.CreatePersonalSettingsSteward>()
      .Add<UcInit.ProvidePlantWithSteward>()
      .Add<UcInit.AssignPlantBox>();

    services.AddPipeline<RareInit.InitPlantRareCommandsArgs>()
      .Add<RareInit.CollectRareCommands>();

    services.AddPipeline<MainVm.GetMainVMPipelineArgs>()
      .Add<MainVm.CreateViewModel>()
      .Add<MainVm.ResolvePlantsConfigVM>()
      .Add<Configuration.ApplicationConfiguration.Injection.InjectApplicationConfigLink>()
      .Add<MainVm.InitializeFirstStep>();

    services.AddPipeline<SinglePlant.ResolveSinglePlantVMPipelineArgs>()
      .Add<SinglePlant.CreatePlantVM>()
      .Add<SinglePlant.StandaloneIconServicePresenter>()
      .Add<SinglePlant.GlobalMenuServiceMenuEmbeddingPresenter>()
      .Add<SinglePlant.GlobalMenuServiceIconChangePresenter>()
      .Add<SinglePlant.ClipboardListenerPresenter>()
      .Add<Services.PlantServices.UserNotifications.Core.Integration.UserNotificationsPresenter>()
      .Add<Services.PlantServices.RareCommands.UI.RareCommandsPresenter>()
      .Add<Services.PlantServices.UserConfig.UI.Intergration.UserConfigPresenter>();

    services.AddPipeline<UcStep.GetUCStepPipelineArgs>()
      .Add<UcStep.ResolveConfigurationEntries>()
      .Add<UcStep.ResolveContentVM>()
      .Add<UcStep.AddResetAllHelpAction>()
      .Add<UcStep.CreateStepInfo>();

    services.AddPipeline<ServicesCfg.GetStateForServicesConfigurationPipelineArgs>()
      .Add<ServicesCfg.InitializeGeneralSettings>()
      .Add<ServicesCfg.ResolveConfigurationEntries>()
      .Add<Services.PlantServices.UserNotifications.Core.Integration.PlantServiceConfigurator>()
      .Add<ServicesCfg.CreateConfigurationVM>()
      .Add<ServicesCfg.MakeResetAllCommandVisible>()
      .Add<ServicesCfg.CreateWindowWithBackState>();

    services.AddPipeline<AppConfig.GetApplicationConfigStepArgs>()
      .Add<AppConfig.AssignVisibleText>()
      .Add<AppConfig.AddRunAtStartupSetting>()
      .Add<AppConfig.CreateConfigurationVM>()
      .Add<Services.Engine.UI.Intergration.ServicesConfigurationInjectSAInjector>()
      .Add<Plants.Intergration.AutoLoadAssembliesSetting>()
      .Add<UI.WindowWithReturn.Integration.ExitOnCloseSetting>()
      .Add<DummyTests.DummySettingInjection>()
      .Add<AppConfig.CreateStep>();

    services.AddPipeline<UnStep.UNConfigurationStepArgs>()
      .Add<UnStep.TuneConfigurationProperties>()
      .Add<UnStep.TuneWindowProperties>()
      .Add<UnStep.AddConfigurationEntries>()
      .Add<UnStep.CreateConfigurationControl>()
      .Add<UnStep.CreateStep>();

    return services;
  }
}
