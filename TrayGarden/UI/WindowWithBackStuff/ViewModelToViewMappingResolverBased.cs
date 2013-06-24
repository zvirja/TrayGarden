using System;
using System.Windows.Controls;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;

namespace TrayGarden.UI.WindowWithBackStuff
{
    public class ViewModelToViewMappingResolverBased : IViewModelToViewMapping
    {
        protected Type _acceptableViewModelType;
        protected Func<Control> _resoler;

        public virtual Type AcceptableViewModelType
        {
            get { return _acceptableViewModelType; }
            protected set { _acceptableViewModelType = value; }
        }

        protected Func<Control> Resoler
        {
            get { return _resoler; }
            set { _resoler = value; }
        }

        public ViewModelToViewMappingResolverBased([NotNull] Type acceptableViewModelType,
                                                   [NotNull] Func<Control> resolver)

        {
            Assert.ArgumentNotNull(acceptableViewModelType, "acceptableViewModelType");
            Assert.ArgumentNotNull(resolver, "resolver");
            _acceptableViewModelType = acceptableViewModelType;
            _resoler = resolver;
        }



        public virtual Control GetControl()
        {
            return Resoler();
        }
    }
}