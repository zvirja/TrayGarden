using System;
using System.Windows;
using System.Windows.Input;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.Services.PlantServices.RareCommands.Core;
using TrayGarden.TypesHatcher;
using TrayGarden.UI;

namespace TrayGarden.Services.PlantServices.RareCommands.UI;

public class RareCommandWrapper : ICommand
{
  public RareCommandWrapper([NotNull] IRareCommand rareCommand)
  {
    Assert.ArgumentNotNull(rareCommand, "rareCommand");
    RareCommand = rareCommand;
  }

  public event EventHandler CanExecuteChanged;

  protected IRareCommand RareCommand { get; set; }

  public virtual bool CanExecute(object parameter)
  {
    return true;
  }

  public virtual void Execute(object parameter)
  {
    try
    {
      RareCommand.ActionToPerform();
    }
    catch (Exception ex)
    {
      Log.Error("Command {0} failed with exception. Delegate of type: {1}".FormatWith(RareCommand.Title, RareCommand.ActionToPerform.Method.Name), ex, this);
      HatcherGuide<IUIManager>.Instance.OKMessageBox(
        "Command failed",
        "Command {0} failed with exception '{1}'".FormatWith(RareCommand.Title, ex.Message),
        MessageBoxImage.Error);
    }
  }
}