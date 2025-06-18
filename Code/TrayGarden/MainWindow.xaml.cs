using System.Windows;

using TrayGarden.Helpers.ThreadSwitcher;
using TrayGarden.TypesHatcher;
using TrayGarden.UI;

namespace TrayGarden;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
  public MainWindow()
  {
    this.InitializeComponent();
    this.Closing += this.MainWindow_Closing;

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

    /* var sa = new SomeArgs { Name = "Pfff" };
           HatcherGuide<IPipelineManager>.Instance.InvokePipeline("simple", sa);

           var rs = sa.Result;*/

    /*var gardenbed = HatcherGuide<IGardenbed>.Instance;
          var firstPlant = gardenbed.GetAllPlants().FirstOrDefault();
          var key = "empty";
          firstPlant.PutLuggage(key,new object());

          var contains = firstPlant.HasLuggage(key);
          var luggage = firstPlant.GetLuggage(key);*/

    /*var servicesSteward = HatcherGuide<IServicesSteward>.Instance;
          servicesSteward.InformInitializeStage();
          servicesSteward.InformDisplayStage();

          var plant = HatcherGuide<IGardenbed>.Instance.GetAllPlants()[0];
          plant.IsEnabled = true;*/

    //  Log.Info("Test info", this);

    // RuntimeHelpers.PrepareMethod(typeof (FileAppender).GetMethod("ActivateOptions").MethodHandle);

    // servicesSteward.InformClosingStage();

    HatcherGuide<IUIManager>.Instance.OKMessageBox("Bye", "Bye");
    //int a = 19;
  }

  private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
  {
    // HatcherGuide<IServicesSteward>.Instance.InformClosingStage();
  }
}

internal class IntSv : Switcher<int>
{
  public IntSv(int newValue)
    : base(newValue)
  {
  }
}