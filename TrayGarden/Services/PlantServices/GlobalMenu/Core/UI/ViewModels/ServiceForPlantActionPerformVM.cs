using System.Windows.Input;
using JetBrains.Annotations;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels
{
    public class ServiceForPlantActionPerformVM : ServiceForPlantVMBase
    {
        private ICommand _performServiceAction;
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

        public ServiceForPlantActionPerformVM([NotNull] string serviceName, [NotNull] string description) : base(serviceName, description)
        {
        }


    }
}