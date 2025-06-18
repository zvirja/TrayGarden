using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.Views;

/// <summary>
/// Interaction logic for PlantConfig.xaml
/// </summary>
public partial class PlantConfig : UserControl
{
  private static string mockDesc =
    "Short description of plant. For instance, here you may find info what plant do and why it's recommended to not disable it";

  public PlantConfig()
  {
    this.InitializeComponent();
    /*var plantVM = new PlantsConfigVM() {IsEnabled = false, Name = "Mock plant name", Description = mockDesc};
          plantVM.ServicesVM = new ObservableCollection<ServiceForPlantVMBase>();
          plantVM.ServicesVM.Add(new ServiceForPlantActionPerformVM("testService", "some desc"));
          plantVM.ServicesVM.Add(new ServiceForPlantWithEnablingVM("testService", "some desc"));
          plantVM.ServicesVM.Add(new ServiceForPlantVMBase("testService", "some desc"));
          plantVM.ServicesVM.Add(new ServiceForPlantWithEnablingVM("testService", "some desc"));

          PlantItems.ItemsSource = new List<PlantsConfigVM>() { plantVM, plantVM, plantVM, plantVM };*/
  }
}