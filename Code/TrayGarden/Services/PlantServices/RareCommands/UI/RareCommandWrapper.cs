using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.Services.PlantServices.RareCommands.Core;
using TrayGarden.TypesHatcher;
using TrayGarden.UI;

namespace TrayGarden.Services.PlantServices.RareCommands.UI
{
  public class RareCommandWrapper : ICommand
  {
    public RareCommandWrapper([NotNull] IRareCommand rareCommand)
    {
      Assert.ArgumentNotNull(rareCommand, "rareCommand");
      this.RareCommand = rareCommand;
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
        this.RareCommand.ActionToPerform();
      }
      catch (Exception ex)
      {
        Log.Error("Command {0} failed with exception. Delegate of type: {1}".FormatWith(this.RareCommand.Title, this.RareCommand.ActionToPerform.Method.Name), ex, this);
        HatcherGuide<IUIManager>.Instance.OKMessageBox(
          "Command failed",
          "Command {0} failed with exception '{1}'".FormatWith(this.RareCommand.Title, ex.Message),
          MessageBoxImage.Error);
      }
    }
  }
}