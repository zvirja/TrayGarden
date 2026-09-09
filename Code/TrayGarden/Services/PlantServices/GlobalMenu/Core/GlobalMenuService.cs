using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using JetBrains.Annotations;

using Microsoft.Extensions.Options;

using TrayGarden.Configuration.Options;
using TrayGarden.Diagnostics;
using TrayGarden.Pipelines.Engine;
using TrayGarden.Plants;
using TrayGarden.Resources;
using TrayGarden.RuntimeSettings;
using TrayGarden.Services.FleaMarket.IconChanger;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.ContextMenuCollecting;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.DynamicState;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.InitPlantPipeline;
using TrayGarden.UI.MainWindow;

using Application = System.Windows.Application;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core;

[UsedImplicitly]
public class GlobalMenuService : PlantServiceBase<GlobalMenuPlantBox>
{
  private readonly IGardenbed _gardenbed;

  private readonly IResourcesManager _resourcesManager;

  private readonly IDynamicStateWatcherFactory _dynamicStateWatcherFactory;

  private readonly INotifyIconChangerFactory _notifyIconChangerFactory;

  private readonly IMainWindowDisplayer _mainWindowDisplayer;

  private readonly IPipelineRunner _pipelineRunner;

  public GlobalMenuService(
    IRuntimeSettingsManager runtimeSettingsManager,
    ContextMenuBuilder contextMenuBuilder,
    IGardenbed gardenbed,
    IResourcesManager resourcesManager,
    IDynamicStateWatcherFactory dynamicStateWatcherFactory,
    INotifyIconChangerFactory notifyIconChangerFactory,
    IMainWindowDisplayer mainWindowDisplayer,
    IPipelineRunner pipelineRunner,
    IOptions<TrayGardenOptions> options)
    : base(runtimeSettingsManager, "Global Menu", "GlobalMenuService")
  {
    _gardenbed = gardenbed;
    _resourcesManager = resourcesManager;
    _dynamicStateWatcherFactory = dynamicStateWatcherFactory;
    _notifyIconChangerFactory = notifyIconChangerFactory;
    _mainWindowDisplayer = mainWindowDisplayer;
    _pipelineRunner = pipelineRunner;
    ContextMenuBuilder = contextMenuBuilder;
    IconText = "Tray Garden";
    TrayIconResourceName = options.Value.GlobalMenu.TrayIconResourceName;
    ServiceDescription = "Service displays the main tray icon. May provide plants with ability to embed their own context menu entries. This service cannot be disabled";
  }

  public override bool CanBeDisabled
  {
    get
    {
      return false;
    }
  }

  public ContextMenuBuilder ContextMenuBuilder { get; set; }

  public string IconText { get; set; }

  public string TrayIconResourceName { get; set; }

  protected NotifyIcon GlobalNotifyIcon { get; set; }

  public override void InformClosingStage()
  {
    base.InformClosingStage();
    if (GlobalNotifyIcon != null)
    {
      GlobalNotifyIcon.Dispose();
    }
  }

  public override void InformDisplayStage()
  {
    base.InformDisplayStage();
    var plantBoxes = new List<GlobalMenuPlantBox>();
    List<IPlantEx> allPlants = _gardenbed.GetAllPlants();
    foreach (IPlantEx plant in allPlants)
    {
      GlobalMenuPlantBox luggage = GetPlantLuggage(plant);
      if (luggage == null)
      {
        continue;
      }
      // Adding a separator to separate plants from one another:
      luggage.ToolStripMenuItems.Add(new ToolStripSeparator());
      plantBoxes.Add(luggage);
      luggage.FixVisibility();
    }

    GlobalNotifyIcon.ContextMenuStrip = BuildContextMenu(plantBoxes);
    GlobalNotifyIcon.Visible = true;
  }

  public override void InformInitializeStage()
  {
    base.InformInitializeStage();
    CreateNotifyIcon();
  }

  public override void InitializePlant(IPlantEx plantEx)
  {
    base.InitializePlant(plantEx);
    InitializePlantFromPipeline(plantEx);
  }

  protected virtual ContextMenuStrip BuildContextMenu(List<GlobalMenuPlantBox> plantBoxes)
  {
    Assert.IsNotNull(ContextMenuBuilder, "Builder cannot be null, something is wrong");
    ContextMenuBuilder.ConfigureContextItemOnClick = ConfigureContextItemOnClick;
    ContextMenuBuilder.ExitContextItemOnClick = ExitContextItemOnClick;
    IDynamicStateWatcher stateWatcher = _dynamicStateWatcherFactory.Create();
    return ContextMenuBuilder.BuildContextMenu(plantBoxes, stateWatcher);
  }

  protected virtual void ConfigureContextItemOnClick(object sender, EventArgs eventArgs)
  {
    OpenConfigurationWindow();
  }

  protected virtual void CreateNotifyIcon()
  {
    GlobalNotifyIcon = new NotifyIcon { Visible = false };
    GlobalNotifyIcon.Text = IconText;
    GlobalNotifyIcon.Icon = GetIcon();
    GlobalNotifyIcon.MouseClick += GlobalNotifyIcon_MouseClick;
  }

  protected virtual void ExitContextItemOnClick(object sender, EventArgs eventArgs)
  {
    Log.For(this).Information("Tray 'Exit Garden' clicked. Calling Application.Shutdown().");
    Application.Current.Shutdown();
  }

  protected virtual Icon GenerateIcon()
  {
    var newIcon = new Bitmap(32, 32);
    var rand = new Random();
    for (int i = 0; i < 500; i++)
    {
      newIcon.SetPixel(rand.Next(31), rand.Next(31), Color.YellowGreen);
    }
    for (int i = 0; i < 250; i++)
    {
      newIcon.SetPixel(rand.Next(31), rand.Next(31), Color.Tomato);
    }
    IntPtr iconHandle = newIcon.GetHicon();
    return Icon.FromHandle(iconHandle);
  }

  protected virtual Icon GetIcon()
  {
    Icon iconResource = _resourcesManager.GetIconResource(TrayIconResourceName, null);
    if (iconResource != null)
    {
      return iconResource;
    }
    return GenerateIcon();
  }

  protected virtual void GlobalNotifyIcon_MouseClick(object sender, MouseEventArgs e)
  {
    if (e.Button == MouseButtons.Left)
    {
      OpenConfigurationWindow();
    }
  }

  protected virtual void InitializePlantFromPipeline(IPlantEx plantEx)
  {
    INotifyIconChangerMaster globalNotifyIconChanger = _notifyIconChangerFactory.Create();
    globalNotifyIconChanger.Initialize(GlobalNotifyIcon);
    _pipelineRunner.Run(new InitPlantGMArgs(plantEx, LuggageName, globalNotifyIconChanger));
  }

  protected virtual void OpenConfigurationWindow()
  {
    _mainWindowDisplayer.PopupMainWindow();
  }

  protected override void PlantOnEnabledChanged(IPlantEx plantEx, bool newValue)
  {
    GlobalMenuPlantBox plantBox = GetPlantLuggage(plantEx);
    if (plantBox == null)
    {
      return;
    }
    plantBox.FixVisibility();
  }
}
