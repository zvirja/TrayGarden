namespace TrayGarden.UI.Common;

public interface IDataTemplateSelector
{
  System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container);
}