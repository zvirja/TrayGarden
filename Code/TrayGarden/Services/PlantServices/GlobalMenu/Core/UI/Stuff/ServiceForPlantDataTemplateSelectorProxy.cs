using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

using TrayGarden.Diagnostics;
using TrayGarden.TypesHatcher;
using TrayGarden.UI.Common;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.Stuff;

public class ServiceForPlantDataTemplateSelectorProxy : DataTemplateSelector, IDataTemplateSelector
{
  public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
  {
    var selector = HatcherGuide<IDataTemplateSelector>.Instance;
    if (selector != null)
    {
      DataTemplate resolvedTemplate = selector.SelectTemplate(item, container);
      if (resolvedTemplate != null && resolvedTemplate != DependencyProperty.UnsetValue)
      {
        return resolvedTemplate;
      }
    }
    var asFrameworkElement = container as FrameworkElement;
    Assert.IsNotNull(asFrameworkElement, "Strange.. passed dependency object isn't framework element");
    DataTemplate defaultDataTemplate = asFrameworkElement.TryFindResource("DefaultMode") as DataTemplate;
    return defaultDataTemplate ?? base.SelectTemplate(item, container);
  }
}