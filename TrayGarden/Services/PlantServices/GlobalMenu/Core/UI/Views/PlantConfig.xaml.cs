using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.Views
{



    /// <summary>
    /// Interaction logic for PlantConfig.xaml
    /// </summary>
    public partial class PlantConfig : UserControl
    {
        private static string mockDesc = "Short description of plant. For instance, here you may find info what plant do and why it's recommended to not disable it";

        public PlantConfig()
        {
            InitializeComponent();
            /*var plantVM = new PlantsConfigVM() {IsEnabled = false, Name = "Mock plant name", Description = mockDesc};
            plantVM.ServicesVM = new ObservableCollection<ServiceForPlantVMBase>();
            plantVM.ServicesVM.Add(new ServiceForPlantActionPerformVM("testService", "some desc"));
            plantVM.ServicesVM.Add(new ServiceForPlantWithEnablingVM("testService", "some desc"));
            plantVM.ServicesVM.Add(new ServiceForPlantVMBase("testService", "some desc"));
            plantVM.ServicesVM.Add(new ServiceForPlantWithEnablingVM("testService", "some desc"));

            PlantItems.ItemsSource = new List<PlantsConfigVM>() { plantVM, plantVM, plantVM, plantVM };*/
        }

        private void ContainerForCheckBox_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            var gridSender = sender as Grid;
            var elem = gridSender.Children.Cast<UIElement>().First(x => x.GetType() == typeof(CheckBox)) as CheckBox;
            var isch = elem.GetValue(CheckBox.IsCheckedProperty);
            var i = 19;
        }
    }
}
