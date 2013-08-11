using System;
using System.Windows.Input;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels
{
    public class ServiceForPlantActionPerformVM : ServiceForPlantVMBase
    {
        protected ICommand _performServiceAction;
        public ICommand PerformServiceAction
        {
            get { return _performServiceAction; }
            set
            {
                if (Equals(value, _performServiceAction)) return;
                _performServiceAction = value;
                OnPropertyChanged("PerformServiceAction");
            }
        }

        public ServiceForPlantActionPerformVM([NotNull] string serviceName, [NotNull] string description,
                                              [NotNull] ICommand action) : base(serviceName, description)
        {
            Assert.ArgumentNotNull(action, "action");
            _performServiceAction = action;
        }
    }
}