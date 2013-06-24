using System;
using System.Windows.Controls;

namespace TrayGarden.UI.WindowWithBackStuff
{
    public interface IViewModelToViewMapping
    {
        Type ViewModelType { get; set; }
        Control GetControl();
    }
}