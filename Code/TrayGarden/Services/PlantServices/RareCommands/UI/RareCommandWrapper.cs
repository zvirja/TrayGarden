using System;
using System.Windows;
using System.Windows.Input;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.Services.PlantServices.RareCommands.Core;
using TrayGarden.UI;

namespace TrayGarden.Services.PlantServices.RareCommands.UI;

public class RareCommandWrapper : ICommand
{
  private readonly IUIManager _uiManager;

  public RareCommandWrapper(IUIManager uiManager, [NotNull] IRareCommand rareCommand)
  {
    Assert.ArgumentNotNull(rareCommand, "rareCommand");
    _uiManager = uiManager;
    RareCommand = rareCommand;
  }

  public event EventHandler CanExecuteChanged;

  private IRareCommand RareCommand { get; set; }

  public bool CanExecute(object parameter)
  {
    return true;
  }

  public void Execute(object parameter)
  {
    try
    {
      RareCommand.ActionToPerform();
    }
    catch (Exception ex)
    {
      Log.For(this).Error(ex, "Command {CommandTitle} failed with exception. Delegate of type: {DelegateType}", RareCommand.Title, RareCommand.ActionToPerform.Method.Name);
      _uiManager.OKMessageBox(
        "Command failed",
        "Command {0} failed with exception '{1}'".FormatWith(RareCommand.Title, ex.Message),
        MessageBoxImage.Error);
    }
  }
}
