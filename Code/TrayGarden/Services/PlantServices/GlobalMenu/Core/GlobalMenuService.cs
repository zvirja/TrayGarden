using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Plants;
using TrayGarden.Resources;
using TrayGarden.Services.FleaMarket.IconChanger;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.ContextMenuCollecting;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.DynamicState;
using TrayGarden.TypesHatcher;
using TrayGarden.UI.MainWindow;

using Application = System.Windows.Application;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core;

[UsedImplicitly]
public class GlobalMenuService : PlantServiceBase<GlobalMenuPlantBox>
{
  public GlobalMenuService()
    : base("Global Menu", "GlobalMenuService")
  {
    this.IconText = "Tray Garden";
    this.TrayIconResourceName = "gardenIcon";
    this.ServiceDescription = "Service displays the main tray icon. May provide plants with ability to embed their own context menu entries. This service cannot be disabled";
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

  protected bool Initialized { get; set; }

  public override void InformClosingStage()
  {
    this.EnsureInitialized();
    base.InformClosingStage();
    if (this.GlobalNotifyIcon != null)
    {
      this.GlobalNotifyIcon.Dispose();
    }
  }

  public override void InformDisplayStage()
  {
    this.EnsureInitialized();
    base.InformDisplayStage();
    var plantBoxes = new List<GlobalMenuPlantBox>();
    List<IPlantEx> allPlants = HatcherGuide<IGardenbed>.Instance.GetAllPlants();
    foreach (IPlantEx plant in allPlants)
    {
      GlobalMenuPlantBox luggage = this.GetPlantLuggage(plant);
      if (luggage == null)
      {
        continue;
      }
      // Adding a separator to separate plants from one another:
      luggage.ToolStripMenuItems.Add(new ToolStripSeparator());
      plantBoxes.Add(luggage);
      luggage.FixVisibility();
    }

    this.GlobalNotifyIcon.ContextMenuStrip = this.BuildContextMenu(plantBoxes);
    this.GlobalNotifyIcon.Visible = true;
  }

  public override void InformInitializeStage()
  {
    this.EnsureInitialized();
    base.InformInitializeStage();
    this.CreateNotifyIcon();
  }

  [UsedImplicitly]
  public virtual void Initialize([NotNull] ContextMenuBuilder builder)
  {
    Assert.ArgumentNotNull(builder, "builder");
    this.ContextMenuBuilder = builder;
    this.Initialized = true;
  }

  public override void InitializePlant(IPlantEx plantEx)
  {
    this.EnsureInitialized();
    base.InitializePlant(plantEx);
    this.InitializePlantFromPipeline(plantEx);
  }

  protected virtual ContextMenuStrip BuildContextMenu(List<GlobalMenuPlantBox> plantBoxes)
  {
    Assert.IsNotNull(this.ContextMenuBuilder, "Builder cannot be null, something is wrong");
    this.ContextMenuBuilder.ConfigureContextItemOnClick = this.ConfigureContextItemOnClick;
    this.ContextMenuBuilder.ExitContextItemOnClick = this.ExitContextItemOnClick;
    IDynamicStateWatcher stateWatcher = HatcherGuide<IDynamicStateWatcher>.CreateNewInstance();
    return this.ContextMenuBuilder.BuildContextMenu(plantBoxes, stateWatcher);
  }

  protected virtual void ConfigureContextItemOnClick(object sender, EventArgs eventArgs)
  {
    this.OpenConfigurationWindow();
  }

  protected virtual void CreateNotifyIcon()
  {
    this.GlobalNotifyIcon = new NotifyIcon { Visible = false };
    this.GlobalNotifyIcon.Text = this.IconText;
    this.GlobalNotifyIcon.Icon = this.GetIcon();
    this.GlobalNotifyIcon.MouseClick += this.GlobalNotifyIcon_MouseClick;
  }

  protected virtual void EnsureInitialized()
  {
    if (!this.Initialized)
    {
      throw new NonInitializedException();
    }
  }

  protected virtual void ExitContextItemOnClick(object sender, EventArgs eventArgs)
  {
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
    IResourcesManager resourceManager = HatcherGuide<IResourcesManager>.Instance;
    Icon iconResource = resourceManager.GetIconResource(this.TrayIconResourceName, null);
    if (iconResource != null)
    {
      return iconResource;
    }
    return this.GenerateIcon();
  }

  protected virtual void GlobalNotifyIcon_MouseClick(object sender, MouseEventArgs e)
  {
    if (e.Button == MouseButtons.Left)
    {
      this.OpenConfigurationWindow();
    }
  }

  protected virtual void InitializePlantFromPipeline(IPlantEx plantEx)
  {
    INotifyIconChangerMaster globalNotifyIconChanger = HatcherGuide<INotifyIconChangerMaster>.CreateNewInstance();
    globalNotifyIconChanger.Initialize(this.GlobalNotifyIcon);
    InitPlantPipeline.InitPlantGMPipeline.Run(plantEx, this.LuggageName, globalNotifyIconChanger);
  }

  protected virtual void OpenConfigurationWindow()
  {
    HatcherGuide<IMainWindowDisplayer>.Instance.PopupMainWindow();
  }

  protected override void PlantOnEnabledChanged(IPlantEx plantEx, bool newValue)
  {
    GlobalMenuPlantBox plantBox = this.GetPlantLuggage(plantEx);
    if (plantBox == null)
    {
      return;
    }
    plantBox.FixVisibility();
  }
}