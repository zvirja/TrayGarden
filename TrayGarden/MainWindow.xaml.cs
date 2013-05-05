using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;
using TrayGarden.Configuration;
using TrayGarden.Features.Contracts;
using TrayGarden.Features.RuntimeSettings;
using TrayGarden.Features.RuntimeSettings.Provider;
using TrayGarden.Helpers;
using TrayGarden.Pipelines.Engine;
using TrayGarden.Pipelines.Simple;
using TrayGarden.Resources;
using TrayGarden.TypesHatcher;

namespace TrayGarden
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            /*
            var bucket = new Bucket(){Name = "Parent"};
            bucket.Settings.Add(new StringStringPair("first","hello"));
            bucket.Settings.Add(new StringStringPair("second", "sec"));


            var b2 = new Bucket() {Name = "ChildB"};
            bucket.InnerBuckets.Add(b2);
            b2.Settings.Add(new StringStringPair("second", "sec"));

            var b3 = new Bucket() {Name = "ChildC"};
            b2.InnerBuckets.Add(b3);
            b3.Settings.Add(new StringStringPair("daddy","moom"));

            var b4 = new Bucket() {Name = "ClildD"};
            b2.InnerBuckets.Add(b4);


            var xmlSer = new XmlSerializer(typeof(Bucket));
            var ms = new MemoryStream();

            xmlSer.Serialize(ms,bucket as Bucket);

            ms.Seek(0, SeekOrigin.Begin);
            var doc = new XmlDocument();
            doc.Load(ms);

            var sb = new StringBuilder();
            using (var xmlWriter = XmlWriter.Create(sb))
            {
                doc.WriteTo(xmlWriter);
            }

            var str = sb.ToString();
            

            var factory = new ContainerFactory();
            var ss =
                Factory.ActualFactory.RawFactory.GetObjectFromConfigurationNode<SettingsStorage>(
                    "trayGarden/features/runtimeSettings/manager/settingsStorageProvider");

            ss.Initialize(factory);
            ss.LoadSettings();
            var root = ss.GetRootContainer();

            var res = ss.SaveSettings();

            

            var container = HatcherContainer<ISettingsStorage>.Instance;
            container.LoadSettings();
            var root = container.GetRootContainer();

             * 
             */


            //var mock = Factory.Instance.GetObject<IPipelineManager>("pipelineManager");
            //var sa = new SomeArgs {Name = "Pfff"};
            //mock.InvokePipeline("simple", sa);
            //var res = sa.Result;

            //var resm = Factory.Instance.GetObject<IResourcesManager>("resourceManager");

            //var str = resm.GetStringResource("qq","NO");

        }
    }
}