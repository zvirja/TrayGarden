using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DevDashboardPlant;

internal static class DevDashboardResources
{
  private const string SvgResourceName = "DevDashboardPlant.Resources.dev-dashboard.svg";
  private const double SvgViewBoxSize = 24;

  public static Icon LoadTrayIcon()
  {
    using var stream = new MemoryStream(RenderPng(32));
    using var bitmap = new Bitmap(stream);
    return Icon.FromHandle(bitmap.GetHicon());
  }

  public static BitmapSource LoadWindowIcon()
  {
    using var stream = new MemoryStream(RenderPng(64));
    return BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
  }

  /// <summary>
  /// Renders the single-path SVG without an SVG library. Only the <c>d</c> and <c>fill</c> attributes of the
  /// first <c>path</c> are honoured, and the viewBox is assumed to be 24x24.
  /// </summary>
  private static byte[] RenderPng(int size)
  {
    using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(SvgResourceName)
      ?? throw new InvalidOperationException($"Embedded resource '{SvgResourceName}' was not found.");
    string svg = new StreamReader(stream).ReadToEnd();

    string pathData = Regex.Match(svg, "\\sd=\"([^\"]+)\"").Groups[1].Value;
    string fill = Regex.Match(svg, "fill=\"([^\"]+)\"").Groups[1].Value;

    // The "F1" prefix selects the non-zero fill rule, which is the SVG default.
    Geometry geometry = Geometry.Parse("F1" + pathData);
    var brush = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString(fill);

    var visual = new DrawingVisual();
    using (DrawingContext context = visual.RenderOpen())
    {
      double scale = size / SvgViewBoxSize;
      context.PushTransform(new ScaleTransform(scale, scale));
      context.DrawGeometry(brush, null, geometry);
    }

    var target = new RenderTargetBitmap(size, size, 96, 96, PixelFormats.Pbgra32);
    target.Render(visual);

    var encoder = new PngBitmapEncoder();
    encoder.Frames.Add(BitmapFrame.Create(target));
    using var output = new MemoryStream();
    encoder.Save(output);
    return output.ToArray();
  }
}
