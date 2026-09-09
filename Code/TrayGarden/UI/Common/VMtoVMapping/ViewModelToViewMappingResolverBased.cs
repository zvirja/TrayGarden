using System;
using System.Windows.Controls;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;

namespace TrayGarden.UI.Common.VMtoVMapping;

public class ViewModelToViewMappingResolverBased : IViewModelToViewMapping
{
  private Type _acceptableViewModelType;

  private Func<object, Control> _resoler;

  public ViewModelToViewMappingResolverBased([NotNull] Type acceptableViewModelType, [NotNull] Func<object, Control> resolver)

  {
    Assert.ArgumentNotNull(acceptableViewModelType, "acceptableViewModelType");
    Assert.ArgumentNotNull(resolver, "resolver");
    _acceptableViewModelType = acceptableViewModelType;
    _resoler = resolver;
  }

  public Type AcceptableViewModelType
  {
    get
    {
      return _acceptableViewModelType;
    }
    private set
    {
      _acceptableViewModelType = value;
    }
  }

  private Func<object, Control> Resoler
  {
    get
    {
      return _resoler;
    }
    set
    {
      _resoler = value;
    }
  }

  public Control GetControl(object contextVM)
  {
    return Resoler(contextVM);
  }
}