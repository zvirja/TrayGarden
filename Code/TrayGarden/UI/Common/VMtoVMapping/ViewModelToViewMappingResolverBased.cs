#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;

#endregion

namespace TrayGarden.UI.Common.VMtoVMapping
{
  public class ViewModelToViewMappingResolverBased : IViewModelToViewMapping
  {
    #region Fields

    protected Type _acceptableViewModelType;

    protected Func<object, Control> _resoler;

    #endregion

    #region Constructors and Destructors

    public ViewModelToViewMappingResolverBased([NotNull] Type acceptableViewModelType, [NotNull] Func<object, Control> resolver)

    {
      Assert.ArgumentNotNull(acceptableViewModelType, "acceptableViewModelType");
      Assert.ArgumentNotNull(resolver, "resolver");
      this._acceptableViewModelType = acceptableViewModelType;
      this._resoler = resolver;
    }

    #endregion

    #region Public Properties

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

    #endregion

    #region Properties

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

    #endregion

    #region Public Methods and Operators

    public virtual Control GetControl(object contextVM)
    {
      return this.Resoler(contextVM);
    }

    #endregion
  }
}