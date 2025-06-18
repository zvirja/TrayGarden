﻿using System;
using System.Collections.Generic;
using System.Drawing;
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
    IconText = "Tray Garden";
    TrayIconResourceName = "gardenIcon";
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

  protected bool Initialized { get; set; }

  public override void InformClosingStage()
  {
    EnsureInitialized();
    base.InformClosingStage();
    if (GlobalNotifyIcon != null)
    {
      GlobalNotifyIcon.Dispose();
    }
  }

  public override void InformDisplayStage()
  {
    EnsureInitialized();
    base.InformDisplayStage();
    var plantBoxes = new List<GlobalMenuPlantBox>();
    List<IPlantEx> allPlants = HatcherGuide<IGardenbed>.Instance.GetAllPlants();
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
    EnsureInitialized();
    base.InformInitializeStage();
    CreateNotifyIcon();
  }

  [UsedImplicitly]
  public virtual void Initialize([NotNull] ContextMenuBuilder builder)
  {
    Assert.ArgumentNotNull(builder, "builder");
    ContextMenuBuilder = builder;
    Initialized = true;
  }

  public override void InitializePlant(IPlantEx plantEx)
  {
    EnsureInitialized();
    base.InitializePlant(plantEx);
    InitializePlantFromPipeline(plantEx);
  }

  protected virtual ContextMenuStrip BuildContextMenu(List<GlobalMenuPlantBox> plantBoxes)
  {
    Assert.IsNotNull(ContextMenuBuilder, "Builder cannot be null, something is wrong");
    ContextMenuBuilder.ConfigureContextItemOnClick = ConfigureContextItemOnClick;
    ContextMenuBuilder.ExitContextItemOnClick = ExitContextItemOnClick;
    IDynamicStateWatcher stateWatcher = HatcherGuide<IDynamicStateWatcher>.CreateNewInstance();
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

  protected virtual void EnsureInitialized()
  {
    if (!Initialized)
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
    Icon iconResource = resourceManager.GetIconResource(TrayIconResourceName, null);
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
    INotifyIconChangerMaster globalNotifyIconChanger = HatcherGuide<INotifyIconChangerMaster>.CreateNewInstance();
    globalNotifyIconChanger.Initialize(GlobalNotifyIcon);
    InitPlantPipeline.InitPlantGMPipeline.Run(plantEx, LuggageName, globalNotifyIconChanger);
  }

  protected virtual void OpenConfigurationWindow()
  {
    HatcherGuide<IMainWindowDisplayer>.Instance.PopupMainWindow();
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