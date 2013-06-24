using System;
using System.Windows.Controls;

namespace TrayGarden.UI.WindowWithBackStuff
{
    public interface IViewModelToViewMapping
    {
        Type AcceptableViewModelType { get; }
        Control GetControl(object contextVM);
    }
}