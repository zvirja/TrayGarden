using System;
using System.Windows.Controls;

using JetBrains.Annotations;

using Microsoft.Extensions.DependencyInjection;

using TrayGarden.Diagnostics;

namespace TrayGarden.UI.Common.VMtoVMapping;

public class ViewModelToViewMappingFactoryBased : IViewModelToViewMapping
{
  private readonly Type _viewType;

  private readonly IServiceProvider _serviceProvider;

  public ViewModelToViewMappingFactoryBased([NotNull] Type sourceType, [NotNull] Type viewType, IServiceProvider serviceProvider)
  {
    Assert.ArgumentNotNull(sourceType, "sourceType");
    Assert.ArgumentNotNull(viewType, "viewType");
    AcceptableViewModelType = sourceType;
    _viewType = viewType;
    _serviceProvider = serviceProvider;
  }

  public Type AcceptableViewModelType { get; }

  public virtual Control GetControl(object contextVM)
  {
    var control = _serviceProvider.GetRequiredService(_viewType) as Control;
    Assert.IsNotNull(control, "Returned value is not Control or is null");
    control.DataContext = contextVM;
    return control;
  }
}
