using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.HelpContent;

public static class WindowPropertySetter
{
  public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.RegisterAttached(
    "BackgroundColor",
    typeof(Brush),
    typeof(WindowPropertySetter),
    new PropertyMetadata(null, PropertyChangedCallback));

  public static readonly DependencyProperty LeftPositionProperty = DependencyProperty.RegisterAttached(
    "LeftPosition",
    typeof(double),
    typeof(WindowPropertySetter),
    new PropertyMetadata(default(double), PropertyChangedCallback));

  public static readonly DependencyProperty OpacityProperty = DependencyProperty.RegisterAttached(
    "Opacity",
    typeof(double),
    typeof(WindowPropertySetter),
    new PropertyMetadata(1.0, PropertyChangedCallback));

  public static readonly DependencyProperty ReadyToBeClosedProperty = DependencyProperty.RegisterAttached(
    "ReadyToBeClosed",
    typeof(bool),
    typeof(WindowPropertySetter),
    new PropertyMetadata(false, PropertyChangedCallback));

  public static readonly DependencyProperty TopPositionProperty = DependencyProperty.RegisterAttached(
    "TopPosition",
    typeof(double),
    typeof(WindowPropertySetter),
    new PropertyMetadata(default(double), PropertyChangedCallback));

  public static Brush GetBackgroundColor(FrameworkElement element)
  {
    return (Brush)element.GetValue(BackgroundColorProperty);
  }

  public static double GetLeftPosition(Window element)
  {
    return (double)element.GetValue(LeftPositionProperty);
  }

  public static double GetOpacity(FrameworkElement element)
  {
    return (double)element.GetValue(BackgroundColorProperty);
  }

  public static bool GetReadyToBeClosed(FrameworkElement element)
  {
    return (bool)element.GetValue(ReadyToBeClosedProperty);
  }

  public static double GetTopPosition(Window element)
  {
    return (double)element.GetValue(TopPositionProperty);
  }

  public static void SetBackgroundColor(FrameworkElement element, Brush value)
  {
    element.SetValue(BackgroundColorProperty, value);
  }

  public static void SetLeftPosition(Window element, double value)
  {
    element.SetValue(LeftPositionProperty, value);
  }

  public static void SetOpacity(FrameworkElement element, double value)
  {
    element.SetValue(BackgroundColorProperty, value);
  }

  public static void SetReadyToBeClosed(FrameworkElement element, bool value)
  {
    element.SetValue(ReadyToBeClosedProperty, value);
  }

  public static void SetTopPosition(Window element, double value)
  {
    element.SetValue(TopPositionProperty, value);
  }

  private static void PropertyChangedCallback(
    DependencyObject dependencyObject,
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
  {
    var frameworkElem = dependencyObject as FrameworkElement;
    if (frameworkElem == null)
    {
      return;
    }
    var newValue = dependencyPropertyChangedEventArgs.NewValue;
    if (dependencyPropertyChangedEventArgs.Property == BackgroundColorProperty)
    {
      SetPropertyToParentWindow(frameworkElem, Control.BackgroundProperty, newValue);
    }
    if (dependencyPropertyChangedEventArgs.Property == OpacityProperty)
    {
      SetPropertyToParentWindow(frameworkElem, UIElement.OpacityProperty, newValue);
    }
    if (dependencyPropertyChangedEventArgs.Property == ReadyToBeClosedProperty)
    {
      SetPropertyToParentWindow(frameworkElem, NotificationWindow.ReadyToBeClosedProperty, newValue);
    }
    if (dependencyPropertyChangedEventArgs.Property == TopPositionProperty)
    {
      SetPropertyToParentWindow(frameworkElem, Window.TopProperty, newValue);
    }
    if (dependencyPropertyChangedEventArgs.Property == LeftPositionProperty)
    {
      SetPropertyToParentWindow(frameworkElem, Window.LeftProperty, newValue);
    }
  }

  private static void SetPropertyToParentWindow(FrameworkElement currentElement, DependencyProperty property, object newValue)
  {
    var currentIterationElem = currentElement;
    do
    {
      var asWindow = currentIterationElem as Window;
      if (asWindow != null)
      {
        asWindow.SetValue(property, newValue);
        return;
      }
      currentIterationElem = currentIterationElem.Parent as FrameworkElement;
    }
    while (currentIterationElem != null);
  }
}