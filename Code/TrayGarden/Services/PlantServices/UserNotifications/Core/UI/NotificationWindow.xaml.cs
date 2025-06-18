using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications.Controls;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications.ViewModes;
using TrayGarden.UI.Common.Converters;
using TrayGarden.UI.Common.VMtoVMapping;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI
{
  /// <summary>
  /// Interaction logic for NotificationWindow.xaml
  /// </summary>
  public partial class NotificationWindow : Window, INotificationWindow, IVMtoVMappingsSource
  {
    public static readonly DependencyProperty ReadyToBeClosedProperty = DependencyProperty.Register(
      "ReadyToBeClosed",
      typeof(bool),
      typeof(NotificationWindow),
      new PropertyMetadata(default(bool), ReadyToBeClosedChanged));

    public NotificationWindow()
    {
      this.SetBinding(ReadyToBeClosedProperty, new Binding("IsAlive") { Mode = BindingMode.OneWay, Converter = new BooleanNotConverter() });

      //Visibility = Visibility.Visible;

      /*var img =
        ImageHelper.GetBitmapImageFromBitmap(
          HatcherGuide<IResourcesManager>.Instance.GetBitmapResource("undo_icon", null),ImageFormat.Png);

      var vm = new ActionNotificationVM("header, some header", "ACTION", img);
      vm.LayoutType = ImageTextOrder.VerticalImageText;
      vm.ButtonTextDisplayStyle.Margins = new Thickness(5,0,20,0);
      vm.HeaderTextDisplayStyle.Margins = new Thickness(15,0,0,0);

      var vm2 = new YesNoNotificationVM("Please select some" + Environment.NewLine + "special value");
      vm2.HeaderTextOptions.Margins = new Thickness(2);
      vm2.YesButtonImage = img;
      vm2.NoButtonImage = img;

      var vm3 = new InformNotificationVM("HEre is some text to inform", 18);

      DataContext = new NotificationWindowVM(new PositionSize(60,1065,300,120),vm3, "Alex P");

      /*DataContext = new NotificationWindowVM(UserNotificationsConfiguration.ServiceSettingsBox,
                                             new PositionSize() { MandatoryWidth = 300, MandatoryHeight = 120 },
                                             new InformNotificationVM("HEllo form hell",14), "Alex P");#1#



     // InitializeComponent();*/
    }

    public List<IViewModelToViewMapping> Mappings { get; protected set; }

    public bool ReadyToBeClosed
    {
      get
      {
        return (bool)this.GetValue(ReadyToBeClosedProperty);
      }
      set
      {
        this.SetValue(ReadyToBeClosedProperty, value);
      }
    }

    public virtual List<IViewModelToViewMapping> GetMappings()
    {
      return this.Mappings
             ?? new List<IViewModelToViewMapping>()
                  {
                    new ViewModelToViewMappingResolverBased(
                      typeof(InformNotificationVM),
                      o => new InformNotification() { DataContext = o }),
                    new ViewModelToViewMappingResolverBased(
                      typeof(ActionNotificationVM),
                      o => new ActionNotification() { DataContext = o }),
                    new ViewModelToViewMappingResolverBased(
                      typeof(YesNoNotificationVM),
                      o => new YesNoNotification() { DataContext = o }),
                  };
    }

    public virtual void Initialize(List<IViewModelToViewMapping> mappings)
    {
      this.Mappings = mappings;
    }

    public virtual void PrepareAndDisplay(NotificationWindowVM viewModel)
    {
      this.DataContext = viewModel;
      this.InitializeComponent();
      this.Show();
    }

    protected static void ReadyToBeClosedChanged(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
    {
      var newBoolValue = (bool)dependencyPropertyChangedEventArgs.NewValue;
      var senderWindow = (NotificationWindow)dependencyObject;
      if (newBoolValue)
      {
        senderWindow.Close();
      }
    }

    protected override void OnClosed(EventArgs e)
    {
      var datacontextAsDisposable = this.DataContext as IDisposable;
      if (datacontextAsDisposable != null)
      {
        datacontextAsDisposable.Dispose();
      }
      base.OnClosed(e);
    }

    protected override void OnMouseEnter(System.Windows.Input.MouseEventArgs e)
    {
      var dataContextAsNotificationVM = this.DataContext as NotificationWindowVM;
      if (dataContextAsNotificationVM != null)
      {
        dataContextAsNotificationVM.IsPositionLocked = true;
      }
      base.OnMouseEnter(e);
    }

    /* protected override void OnMouseLeave(System.Windows.Input.MouseEventArgs e)
    {
      var dataContextAsNotificationVM = DataContext as NotificationWindowVM;
      if (dataContextAsNotificationVM != null)
        dataContextAsNotificationVM.IsPositionLocked = false;
      base.OnMouseLeave(e);
    }*/
  }
}