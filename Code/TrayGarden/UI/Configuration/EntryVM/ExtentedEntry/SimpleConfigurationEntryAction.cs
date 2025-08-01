﻿using System;
using System.Windows.Input;
using System.Windows.Media;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.UI.Common.Commands;

namespace TrayGarden.UI.Configuration.EntryVM.ExtentedEntry;

public class SimpleConfigurationEntryAction : IConfigurationEntryAction
{
  public SimpleConfigurationEntryAction(
    [NotNull] ImageSource labelImage,
    [NotNull] Action<object> action,
    bool enabled = true,
    object customParam = null,
    string hint = null)
  {
    Assert.ArgumentNotNull(labelImage, "labelImage");
    Assert.ArgumentNotNull(action, "action");
    LabelImage = labelImage;
    Action = new RelayCommand(ExecuteAction, o => Enabled);
    StoredAction = action;
    Enabled = enabled;
    CustomParam = customParam;
    Hint = hint;
  }

  public ICommand Action { get; set; }

  public bool Enabled { get; set; }

  public string Hint { get; set; }

  public ImageSource LabelImage { get; set; }

  protected object CustomParam { get; set; }

  protected Action<object> StoredAction { get; set; }

  protected virtual void ExecuteAction(object o)
  {
    StoredAction(CustomParam);
  }
}