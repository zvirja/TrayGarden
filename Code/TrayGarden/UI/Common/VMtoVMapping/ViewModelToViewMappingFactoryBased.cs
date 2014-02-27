#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Controls;

using JetBrains.Annotations;

using TrayGarden.Configuration;
using TrayGarden.Diagnostics;

#endregion

namespace TrayGarden.UI.Common.VMtoVMapping
{
  public class ViewModelToViewMappingFactoryBased : IViewModelToViewMapping
  {
    #region Constructors and Destructors

    public ViewModelToViewMappingFactoryBased()
    {
      this.Initialized = false;
    }

    #endregion

    #region Public Properties

    public Type AcceptableViewModelType { get; protected set; }

    #endregion

    #region Properties

    protected IObjectFactory ControlFactory { get; set; }

    protected bool Initialized { get; set; }

    #endregion

    #region Public Methods and Operators

    public virtual Control GetControl(object contextVM)
    {
      var syncc = SynchronizationContext.Current;
      this.AssertInitialized();
      var control = this.ControlFactory.GetPurelyNewObject() as Control;
      Assert.IsNotNull(control, "Returned value is not Control or is null");
      control.DataContext = contextVM;
      return control;
    }

    [UsedImplicitly]
    public virtual void Initialize([NotNull] Type sourceType, [NotNull] IObjectFactory controlFactory)
    {
      Assert.ArgumentNotNull(sourceType, "sourceType");
      Assert.ArgumentNotNull(controlFactory, "controlFactory");
      this.AcceptableViewModelType = sourceType;
      this.ControlFactory = controlFactory;
      this.Initialized = true;
    }

    #endregion

    #region Methods

    protected virtual void AssertInitialized()
    {
      if (!this.Initialized)
      {
        throw new NonInitializedException();
      }
    }

    #endregion
  }
}