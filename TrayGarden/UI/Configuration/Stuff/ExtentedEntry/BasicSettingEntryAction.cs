using System;
using System.Windows.Input;
using System.Windows.Media;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;
using TrayGarden.UI.Common.Commands;

namespace TrayGarden.UI.Configuration.Stuff.ExtentedEntry
{
  public class BasicSettingEntryAction : ISettingEntryAction
  {
    protected Action<object> StoredAction { get; set; }
    protected object CustomParam { get; set; }

    public string Hint { get; set; }
    public ICommand Action { get; set; }
    public ImageSource LabelImage { get; set; }
    public bool Enabled { get; set; }

    public BasicSettingEntryAction([NotNull] ImageSource labelImage, [NotNull] Action<object> action, bool enabled = true, object customParam = null, string hint = null)
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

    protected virtual void ExecuteAction(object o)
    {
      StoredAction(CustomParam);
    }
  }
}