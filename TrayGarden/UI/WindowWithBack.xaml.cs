using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;
using TrayGarden.Services.FleaMarket.IconChanger;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.Views;
using TrayGarden.UI.WindowWithBackStuff;

namespace TrayGarden.UI
{
    /// <summary>
    /// Interaction logic for WindowWithBack.xaml
    /// </summary>
    public partial class WindowWithBack : Window, IContentValueConverterMappingsSource, IWindowWithBack
    {
        protected MappingsBasedContentValueConverter ViewModelToViewConverter { get; set; }
        protected List<IViewModelToViewMapping> Mappings { get; set; }


        private int counter = 0;

        public WindowWithBack()
        {
           /* //TODO Remove later
            Mappings = new List<IViewModelToViewMapping>()
                {
                    new ViewModelToViewMappingResolverBased(typeof (object), delegate
                        {
                            return new TextBox() {Text = "HELLO ALEX" + counter++};
                        })
                };

            //END REMOVE LATER*/


            InitializeComponent();
            Hide();
            //Uc.Content = new PlantConfig();

            //DataContext = new WindowWithBackVMBase();

/*           var dc = new WindowWithBackVMBase { ContentVM = this, };
           dc.SelfHelpActions.Add(new ActionCommandVM(new RelayCommand(x => MessageBox.Show("HEllo 1"), true), "Action 1"));
           dc.SelfHelpActions.Add(new ActionCommandVM(new RelayCommand(x => MessageBox.Show("HEllo 3"), true), "Action 3"));

           dc.SelfHelpActions.Add(new ActionCommandVM(new RelayCommand(delegate(object o)
               {
                   var newStep = new WindowWithBackState("Tray garden - Custom FRR", "Custom Frr", "fRR", null, new ActionCommandVM(), 
                                                   null);
                   var rc = new RelayCommand(x => MessageBox.Show("Frr"), true);
                   newStep.StateSpecificHelpActions.Add(new ActionCommandVM(rc, "Hey FRR"));
                   dc.ReplaceInitialState(newStep);
               }, true), "Action Add"));


            this.DataContext = dc;*/




        }

        public virtual void Initialize([NotNull] List<IViewModelToViewMapping> mvtovmappings)
        {
            Assert.ArgumentNotNull(mvtovmappings, "mvtovmappings");
            Mappings = mvtovmappings;
        }

        public virtual void PrepareAndShow(WindowWithBackVMBase viewModel)
        {
            CleanupAndDisposeDataContext();
            this.DataContext = viewModel;
            Show();
        }

        public virtual List<IViewModelToViewMapping> GetMappings()
        {
            return Mappings?? new List<IViewModelToViewMapping>();
        }
       

        protected override void OnClosing(CancelEventArgs e)
        {

            e.Cancel = true;
            Hide();

            CleanupAndDisposeDataContext();
            base.OnClosing(e);
        }

        protected virtual void CleanupAndDisposeDataContext()
        {
            var currentDataContextAsDisposable = DataContext as IDisposable;
            DataContext = null;
            if (currentDataContextAsDisposable != null)
                currentDataContextAsDisposable.Dispose();
        }

        
    }
}
