#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.UI.Common.Commands;

#endregion

namespace TrayGarden.UI.Configuration.EntryVM.ExtentedEntry
{
  public class SimpleConfigurationEntryAction : IConfigurationEntryAction
  {
    #region Constructors and Destructors

    public SimpleConfigurationEntryAction(
      [NotNull] ImageSource labelImage,
      [NotNull] Action<object> action,
      bool enabled = true,
      object customParam = null,
      string hint = null)
    {
      Assert.ArgumentNotNull(labelImage, "labelImage");
      Assert.ArgumentNotNull(action, "action");
      this.LabelImage = labelImage;
      this.Action = new RelayCommand(this.ExecuteAction, o => this.Enabled);
      this.StoredAction = action;
      this.Enabled = enabled;
      this.CustomParam = customParam;
      this.Hint = hint;
    }

    #endregion

    #region Public Properties

    public ICommand Action { get; set; }

    public bool Enabled { get; set; }

    public string Hint { get; set; }

    public ImageSource LabelImage { get; set; }

    #endregion

    #region Properties

    protected object CustomParam { get; set; }

    protected Action<object> StoredAction { get; set; }

    #endregion

    #region Methods

    protected virtual void ExecuteAction(object o)
    {
      this.StoredAction(this.CustomParam);
    }

    #endregion
  }
}