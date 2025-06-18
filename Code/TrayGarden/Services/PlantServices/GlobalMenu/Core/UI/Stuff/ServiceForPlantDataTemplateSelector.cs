using System;
using System.Windows;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;
using TrayGarden.UI.Common;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.Stuff;

[UsedImplicitly]
public class ServiceForPlantDataTemplateSelector : IDataTemplateSelector
{
  public virtual DataTemplate SelectTemplate(object item, DependencyObject container)
  {
    var asFrameworkElement = container as FrameworkElement;
    Assert.IsNotNull(asFrameworkElement, "Strange.. passed dependency object isn't framework element");

    var resolvedDataTemplate = TryResolveFromResources(item, asFrameworkElement);

    string defaultResourceKey = "DefaultMode";
    string resourceKey = GetResourceKey(item) ?? defaultResourceKey;
    var requiredDataTemplate = FindResource(asFrameworkElement, resourceKey);
    return requiredDataTemplate
           ?? (!resourceKey.Equals(defaultResourceKey, StringComparison.OrdinalIgnoreCase)
             ? FindResource(asFrameworkElement, defaultResourceKey)
             : null);
  }

  protected virtual DataTemplate FindResource(FrameworkElement container, string key)
  {
    return container.TryFindResource(key) as DataTemplate;
  }

  protected virtual string GetResourceKey(object vmItem)
  {
    if (vmItem is ServiceForPlantActionPerformVM)
    {
      return "ServiceForPlantActionPerform";
    }
    if (vmItem is ServiceForPlantWithEnablingVM)
    {
      return "ServiceForPlantWithEnabling";
    }
    return null;
  }

  protected virtual DataTemplate TryResolveFromResources(object item, FrameworkElement container)
  {
    var c = container.Resources.Count;
    return null;
  }
}