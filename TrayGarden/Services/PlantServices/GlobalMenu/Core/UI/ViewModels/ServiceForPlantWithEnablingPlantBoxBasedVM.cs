using System;
using JetBrains.Annotations;
using TrayGarden.Services.PlantServices.StandaloneIcon.Core;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels
{

    [UsedImplicitly]
    public class ServiceForPlantWithEnablingPlantBoxBasedVM : ServiceForPlantWithEnablingVM, IDisposable
    {

        protected ServicePlantBoxBase AssignedPlantBox { get; set; }


        [UsedImplicitly]
        public override bool IsEnabled
        {
            get { return AssignedPlantBox.IsEnabled; }
            set
            {
                if (value.Equals(AssignedPlantBox.IsEnabled)) return;
                AssignedPlantBox.IsEnabled = value;
                OnPropertyChanged("IsEnabled");
                OnIsEnabledChanged(value);
            }
        }

        public ServiceForPlantWithEnablingPlantBoxBasedVM([NotNull] string serviceName, [NotNull] string description, ServicePlantBoxBase plantBox)
            : base(serviceName, description)
        {
            AssignedPlantBox = plantBox;
            AssignedPlantBox.IsEnabledChanged += AssignedPlantBox_IsEnabledChanged;
        }

        protected virtual void AssignedPlantBox_IsEnabledChanged(ServicePlantBoxBase sender, bool newValue)
        {
            OnPropertyChanged("IsEnabled");
        }


        public virtual void Dispose()
        {
            AssignedPlantBox.IsEnabledChanged -= AssignedPlantBox_IsEnabledChanged;
        }
    }
}