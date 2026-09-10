using System;
using System.Collections.Generic;
using System.Windows;

using TrayGarden.Reception;
using TrayGarden.Reception.Services;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.ContextMenuCollecting;
using TrayGarden.Services.PlantServices.IsEnabledObserver;

namespace MicrosoftTodoTrayPlant;

public class Plant : IPlant, IServicesDelegation, IIsEnabledObserver, IExtendsGlobalMenu
{
  private IPlantEnabledInfo _isEnabledInfo;

  public string Description => "Minimizes / closes the Microsoft To Do window to a tray icon instead of quitting.";

  public string HumanSupportingName => "Microsoft To Do tray";

  public void Initialize()
  {
  }

  public void PostServicesInitialize()
  {
    if (Application.Current != null)
    {
      Application.Current.Exit += (_, _) => Shutdown();
      Application.Current.SessionEnding += (_, _) => Shutdown();
    }

    if (_isEnabledInfo == null || _isEnabledInfo.IsEnabled)
    {
      WrapperController.Instance.EnableHotKey();
    }
  }

  public List<object> GetServiceDelegates()
  {
    return [TodoTrayIcon.Instance, PlantConfiguration.Instance];
  }

  public bool FillProvidedContextMenuBuilder(IMenuEntriesAppender menuAppender)
  {
    menuAppender.AppentMenuStripItem(
      "Toggle Microsoft To Do",
      PlantResources.LoadTrayIcon(),
      (_, _) => WrapperController.Instance.ToggleShown());
    return true;
  }

  public void ConsumeIsEnabledInfo(IPlantEnabledInfo plantEnabledInfo)
  {
    _isEnabledInfo = plantEnabledInfo;
    _isEnabledInfo.IsEnabledChanged += OnIsEnabledChanged;
  }

  private void OnIsEnabledChanged(object sender, EventArgs e)
  {
    if (_isEnabledInfo.IsEnabled)
    {
      WrapperController.Instance.EnableHotKey();
    }
    else
    {
      WrapperController.Instance.DetachAndRelease();
      WrapperController.Instance.DisableHotKey();
    }
  }

  private static void Shutdown()
  {
    WrapperController.Instance.KillOnShutdown();
  }
}
