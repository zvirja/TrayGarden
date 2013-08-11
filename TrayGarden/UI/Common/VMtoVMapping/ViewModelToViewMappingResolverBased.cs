using System;
using System.Windows.Controls;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;

namespace TrayGarden.UI.Common.VMtoVMapping
{
    public class ViewModelToViewMappingResolverBased : IViewModelToViewMapping
    {
        protected Type _acceptableViewModelType;
        protected Func<object,Control> _resoler;

        public virtual Type AcceptableViewModelType
        {
            get { return _acceptableViewModelType; }
            protected set { _acceptableViewModelType = value; }
        }

        protected Func<object,Control> Resoler
        {
            get { return _resoler; }
            set { _resoler = value; }
        }

        public ViewModelToViewMappingResolverBased([NotNull] Type acceptableViewModelType,
                                                   [NotNull] Func<object, Control> resolver)

        {
            Assert.ArgumentNotNull(acceptableViewModelType, "acceptableViewModelType");
            Assert.ArgumentNotNull(resolver, "resolver");
            _acceptableViewModelType = acceptableViewModelType;
            _resoler = resolver;
        }



        public virtual Control GetControl(object contextVM)
        {
            return Resoler(contextVM);
        }
    }
}