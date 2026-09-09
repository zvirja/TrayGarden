using System;
using System.Collections.Generic;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using TrayGarden.Configuration.Options;
using TrayGarden.Pipelines.Engine;
using TrayGarden.Plants;
using TrayGarden.Resources;
using TrayGarden.RuntimeSettings;
using TrayGarden.RuntimeSettings.Provider;
using TrayGarden.Services;
using TrayGarden.Services.Engine;
using TrayGarden.Services.FleaMarket.IconChanger;
using TrayGarden.Services.PlantServices.ClipboardObserver.Core;
using TrayGarden.Services.PlantServices.CustomSettings.Core;
using TrayGarden.Services.PlantServices.GlobalMenu.Core;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.ContextMenuCollecting;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.DynamicState;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.Stuff;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.Views;
using TrayGarden.Services.PlantServices.IsEnabledObserver;
using TrayGarden.Services.PlantServices.RareCommands.Core;
using TrayGarden.Services.PlantServices.StandaloneIcon.Core;
using TrayGarden.Services.PlantServices.UserConfig.Core;
using TrayGarden.Services.PlantServices.UserNotifications.Core;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Displaying;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Displaying.DisplayProviders;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications.Controls;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications.ViewModes;
using TrayGarden.UI;
using TrayGarden.UI.Common;
using TrayGarden.UI.Common.VMtoVMapping;
using TrayGarden.UI.Configuration;
using TrayGarden.UI.MainWindow;
using TrayGarden.UI.WindowWithReturn;

namespace TrayGarden.Composition;

public static class GardenRegistration
{
  public static IServiceCollection AddGarden(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddOptions<TrayGardenOptions>()
      .Bind(configuration.GetSection(TrayGardenOptions.SectionName));

    services.AddSingleton<IPipelineRunner, PipelineRunner>();
    services.AddGardenPipelines();

    AddCore(services);
    AddServices(services);
    AddBricks(services);
    AddUI(services);

    return services;
  }

  private static void AddCore(IServiceCollection services)
  {
    services.AddTransient<IContainer, Container>();
    services.AddSingleton<IContainerFactory, ContainerFactory>();

    services.AddSingleton<ISettingsStorage, SettingsStorage>();
    services.AddSingleton<IRuntimeSettingsManager, RuntimeSettingsManager>();

    services.AddSingleton<IResourcesManager>(sp =>
    {
      var manager = new MultisourceResourcesManager();
      var options = sp.GetRequiredService<IOptions<TrayGardenOptions>>().Value;
      foreach (ResourceSourceOptions sourceOptions in options.Resources.Sources)
      {
        var source = new AssemblySource();
        source.Initialize(sourceOptions.Assembly, sourceOptions.Path);
        manager.Sources.Add(source);
      }
      return manager;
    });

    services.AddSingleton<Helpers.ISingleInstanceMonitor, Helpers.SingleInstanceMonitor>();
    services.AddSingleton<IGardenbed, Gardenbed>();
    services.AddSingleton<IServicesSteward, ServicesSteward>();
  }

  private static void AddServices(IServiceCollection services)
  {
    // Order matches the historical App.config <services> order; it is the order
    // ServicesSteward walks IEnumerable<IService>.
    RegisterService<CustomSettingsService>(services);
    RegisterService<UserConfigService>(services);
    RegisterService<ClipboardObserverService>(services);
    RegisterService<UserNotificationsService>(services);
    RegisterService<StandaloneIconService>(services);
    RegisterService<GlobalMenuService>(services);
    RegisterService<RareCommandsService>(services);
    RegisterService<IsEnabledObserverService>(services);
  }

  private static void RegisterService<TService>(IServiceCollection services)
    where TService : class, IService
  {
    services.AddSingleton<TService>();
    services.AddSingleton<IService>(sp => sp.GetRequiredService<TService>());
  }

  private static void AddBricks(IServiceCollection services)
  {
    services.AddTransient<IDynamicStateDecorator, DynamicStateDecorator>();
    services.AddTransient<IDynamicStateWatcher, DynamicStateWatcher>();
    services.AddSingleton<IDynamicStateWatcherFactory, DynamicStateWatcherFactory>();

    services.AddTransient<INotifyIconChangerMaster>(sp =>
    {
      var options = sp.GetRequiredService<IOptions<TrayGardenOptions>>().Value;
      return new NotifyIconChanger { DefaultDelayMsec = options.NotifyIconChanger.DefaultDelayMsec };
    });
    services.AddSingleton<INotifyIconChangerFactory, NotifyIconChangerFactory>();

    services.AddSingleton<ContextMenuBuilder>();

    services.AddSingleton<Configuration.ApplicationConfiguration.Autorun.IAutorunHelper,
      Configuration.ApplicationConfiguration.Autorun.AutorunHelper>();

    services.AddSingleton<TopRightCornerProvider>();
    services.AddSingleton<IDisplayQueueProvider>(sp => sp.GetRequiredService<TopRightCornerProvider>());
    services.AddSingleton<IUserNotificationsGate, UserNotificationsGate>();

    services.AddSingleton<IDataTemplateSelector, ServiceForPlantDataTemplateSelector>();
  }

  private static void AddUI(IServiceCollection services)
  {
    services.AddSingleton<IUIManager, UIManager>();
    services.AddSingleton<IMainWindowDisplayer, MainWindowDisplayer>();

    services.AddTransient<PlantConfig>();
    services.AddTransient<ConfigurationControl>();
    services.AddTransient<InformNotification>();
    services.AddTransient<ActionNotification>();
    services.AddTransient<YesNoNotification>();

    services.AddSingleton<IWindowWithBack>(sp => new WindowWithBack(
      sp.GetRequiredService<IResourcesManager>(),
      sp.GetRequiredService<IOptions<TrayGardenOptions>>(),
      new IViewModelToViewMapping[]
      {
        new ViewModelToViewMappingFactoryBased(typeof(PlantsConfigVM), typeof(PlantConfig), sp),
        new ViewModelToViewMappingFactoryBased(typeof(ConfigurationControlVM), typeof(ConfigurationControl), sp)
      }));

    services.AddTransient<INotificationWindow>(sp => new NotificationWindow(
      new IViewModelToViewMapping[]
      {
        new ViewModelToViewMappingFactoryBased(typeof(InformNotificationVM), typeof(InformNotification), sp),
        new ViewModelToViewMappingFactoryBased(typeof(ActionNotificationVM), typeof(ActionNotification), sp),
        new ViewModelToViewMappingFactoryBased(typeof(YesNoNotificationVM), typeof(YesNoNotification), sp)
      }));
    services.AddSingleton<INotificationWindowFactory, NotificationWindowFactory>();
  }
}
