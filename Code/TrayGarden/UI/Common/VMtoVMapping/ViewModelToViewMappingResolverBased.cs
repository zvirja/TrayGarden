using System;
using System.Windows.Controls;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;

namespace TrayGarden.UI.Common.VMtoVMapping;

public class ViewModelToViewMappingResolverBased : IViewModelToViewMapping
{
  protected Type _acceptableViewModelType;

  protected Func<object, Control> _resoler;

  public ViewModelToViewMappingResolverBased([NotNull] Type acceptableViewModelType, [NotNull] Func<object, Control> resolver)

  {
    Assert.ArgumentNotNull(acceptableViewModelType, "acceptableViewModelType");
    Assert.ArgumentNotNull(resolver, "resolver");
    this._acceptableViewModelType = acceptableViewModelType;
    this._resoler = resolver;
  }

  public virtual Type AcceptableViewModelType
  {
    get
    {
      return this._acceptableViewModelType;
    }
    protected set
    {
      this._acceptableViewModelType = value;
    }
  }

  protected Func<object, Control> Resoler
  {
    get
    {
      return this._resoler;
    }
    set
    {
      this._resoler = value;
    }
  }

  public virtual Control GetControl(object contextVM)
  {
    return this.Resoler(contextVM);
  }
}