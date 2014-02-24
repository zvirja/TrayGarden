using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Controls;
using JetBrains.Annotations;
using TrayGarden.Configuration;
using TrayGarden.Diagnostics;

namespace TrayGarden.UI.Common.VMtoVMapping
{
    public class ViewModelToViewMappingFactoryBased : IViewModelToViewMapping
    {
        protected IObjectFactory ControlFactory { get; set; }
        protected bool Initialized { get; set; }

        public Type AcceptableViewModelType { get; protected set; }

        public ViewModelToViewMappingFactoryBased()
        {
            Initialized = false;
        }

        [UsedImplicitly]
        public virtual void Initialize([NotNull] Type sourceType, [NotNull] IObjectFactory controlFactory)
        {
            Assert.ArgumentNotNull(sourceType, "sourceType");
            Assert.ArgumentNotNull(controlFactory, "controlFactory");
            AcceptableViewModelType = sourceType;
            ControlFactory = controlFactory;
            Initialized = true;
        }

        public virtual Control GetControl(object contextVM)
        {
          var syncc = SynchronizationContext.Current;
            AssertInitialized();
            var control = ControlFactory.GetPurelyNewObject() as Control;
            Assert.IsNotNull(control,"Returned value is not Control or is null");
            control.DataContext = contextVM;
            return control;
        }


        protected virtual void AssertInitialized()
        {
            if (!Initialized)
                throw new NonInitializedException();
        }
    }
}
