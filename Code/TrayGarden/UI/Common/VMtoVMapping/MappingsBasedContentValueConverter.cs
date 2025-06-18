using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using TrayGarden.Diagnostics;

namespace TrayGarden.UI.Common.VMtoVMapping;

[ValueConversion(typeof(object), typeof(Control))]
public class MappingsBasedContentValueConverter : IMultiValueConverter
{
  public virtual object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
  {
    // return new TextBox(){Text = value[0].ToString()};

    Assert.IsTrue(value.Length == 2, "Should be 2 params");
    object valueToConvert = value[0];
    //Use self resolving hack for better customization
    Control resolvedUsingSelfResolving = this.ResolveUsingSelfResolving(valueToConvert);
    if (resolvedUsingSelfResolving != null)
    {
      return resolvedUsingSelfResolving;
    }

    //Resolve using trivial method
    var mappingsSource = value[1] as IVMtoVMappingsSource;

    if (valueToConvert == null || valueToConvert == DependencyProperty.UnsetValue || mappingsSource == null
        || mappingsSource == DependencyProperty.UnsetValue)
    {
      return DependencyProperty.UnsetValue;
    }

    List<IViewModelToViewMapping> mappings = mappingsSource.GetMappings();
    Assert.IsNotNull(mappings, "Mappings cannot be null");

    Type valueType = valueToConvert.GetType();
    IViewModelToViewMapping mapping = mappings.FirstOrDefault(x => x.AcceptableViewModelType.IsAssignableFrom(valueType));
    var resolvedMapping = mapping != null ? mapping.GetControl(valueToConvert) : DependencyProperty.UnsetValue;
    return resolvedMapping;
  }

  public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
  {
    throw new NotSupportedException();
  }

  protected virtual Control ResolveUsingSelfResolving(object viewModel)
  {
    var vmAsSelfResolver = viewModel as ISelfViewResolver;
    if (vmAsSelfResolver == null)
    {
      return null;
    }
    return vmAsSelfResolver.GetViewToPresentMe();
  }
}