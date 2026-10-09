using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace UngoogledChromiumUpdaterPlant;

public enum UpdateIconKind
{
  /// <summary>A star: an update is downloaded and waiting to be installed.</summary>
  UpdateReady,

  /// <summary>A down arrow: the installer is being downloaded.</summary>
  Downloading,

  /// <summary>A circular arrow: the installation is in progress.</summary>
  Installing
}

/// <summary>
/// Builds the tray icon: the icon of the installed chrome.exe with a badge in the bottom-right corner.
/// </summary>
public static class UpdateIconFactory
{
  private const int Size = 64;
  private const int StarPoints = 8;
  private const float BadgeCenter = 45f;

  public static Icon Create(string chromeExePath, UpdateIconKind kind)
  {
    using Bitmap bitmap = new(Size, Size);
    using (Graphics graphics = Graphics.FromImage(bitmap))
    {
      graphics.SmoothingMode = SmoothingMode.AntiAlias;
      graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
      graphics.Clear(Color.Transparent);
      using (Icon baseIcon = ExtractBaseIcon(chromeExePath))
      {
        if (baseIcon != null)
        {
          graphics.DrawIcon(baseIcon, new Rectangle(0, 0, Size, Size));
        }
        else
        {
          DrawFallbackBase(graphics);
        }
      }

      switch (kind)
      {
        case UpdateIconKind.Installing:
          DrawInstallingBadge(graphics);
          break;
        case UpdateIconKind.Downloading:
          DrawDownloadingBadge(graphics);
          break;
        default:
          DrawStarBadge(graphics);
          break;
      }
    }

    IntPtr handle = bitmap.GetHicon();
    try
    {
      using Icon unmanaged = Icon.FromHandle(handle);
      return (Icon)unmanaged.Clone();
    }
    finally
    {
      DestroyIcon(handle);
    }
  }

  private static Icon ExtractBaseIcon(string path)
  {
    try
    {
      var handles = new IntPtr[1];
      uint count = PrivateExtractIcons(path, 0, Size, Size, handles, null, 1, 0);
      if (count == 1 && handles[0] != IntPtr.Zero)
      {
        try
        {
          using Icon unmanaged = Icon.FromHandle(handles[0]);
          return (Icon)unmanaged.Clone();
        }
        finally
        {
          DestroyIcon(handles[0]);
        }
      }

      return Icon.ExtractAssociatedIcon(path);
    }
    catch (Exception)
    {
      return null;
    }
  }

  private static void DrawFallbackBase(Graphics graphics)
  {
    using var brush = new SolidBrush(Color.FromArgb(66, 133, 244));
    graphics.FillEllipse(brush, 4, 4, Size - 8, Size - 8);
  }

  private static void DrawStarBadge(Graphics graphics)
  {
    const float centerX = BadgeCenter;
    const float centerY = BadgeCenter;
    const float outerRadius = 19f;
    const float innerRadius = 9f;

    var points = new PointF[StarPoints * 2];
    for (int i = 0; i < points.Length; i++)
    {
      double angle = -Math.PI / 2 + i * Math.PI / StarPoints;
      float radius = i % 2 == 0 ? outerRadius : innerRadius;
      points[i] = new PointF(centerX + radius * (float)Math.Cos(angle), centerY + radius * (float)Math.Sin(angle));
    }

    using var fill = new SolidBrush(Color.FromArgb(255, 193, 7));
    using var outline = new Pen(Color.FromArgb(90, 55, 0), 3f) { LineJoin = LineJoin.Round };
    graphics.DrawPolygon(outline, points);
    graphics.FillPolygon(fill, points);
  }

  private static void DrawProgressDisc(Graphics graphics)
  {
    const float badgeRadius = 19f;
    using var fill = new SolidBrush(Color.FromArgb(46, 160, 67));
    using var outline = new Pen(Color.FromArgb(10, 70, 25), 3f);
    graphics.FillEllipse(fill, BadgeCenter - badgeRadius, BadgeCenter - badgeRadius, badgeRadius * 2, badgeRadius * 2);
    graphics.DrawEllipse(outline, BadgeCenter - badgeRadius, BadgeCenter - badgeRadius, badgeRadius * 2, badgeRadius * 2);
  }

  private static void DrawDownloadingBadge(Graphics graphics)
  {
    DrawProgressDisc(graphics);
    using var shaftPen = new Pen(Color.White, 5f);
    graphics.DrawLine(shaftPen, BadgeCenter, BadgeCenter - 11f, BadgeCenter, BadgeCenter + 2f);
    var head = new[]
    {
      new PointF(BadgeCenter - 9f, BadgeCenter),
      new PointF(BadgeCenter + 9f, BadgeCenter),
      new PointF(BadgeCenter, BadgeCenter + 11f)
    };
    graphics.FillPolygon(Brushes.White, head);
  }

  private static void DrawInstallingBadge(Graphics graphics)
  {
    const float arrowRadius = 9f;
    const float startAngle = -60f;
    const float sweepAngle = 270f;

    DrawProgressDisc(graphics);
    using var arcPen = new Pen(Color.White, 4f);
    graphics.DrawArc(arcPen, BadgeCenter - arrowRadius, BadgeCenter - arrowRadius, arrowRadius * 2, arrowRadius * 2, startAngle, sweepAngle);

    // Arrowhead at the end of the clockwise arc, pointing along the tangent.
    double endAngle = (startAngle + sweepAngle) * Math.PI / 180;
    var normal = new PointF((float)Math.Cos(endAngle), (float)Math.Sin(endAngle));
    var tangent = new PointF(-normal.Y, normal.X);
    var end = new PointF(BadgeCenter + arrowRadius * normal.X, BadgeCenter + arrowRadius * normal.Y);
    var head = new[]
    {
      new PointF(end.X + tangent.X * 7f, end.Y + tangent.Y * 7f),
      new PointF(end.X + normal.X * 6f, end.Y + normal.Y * 6f),
      new PointF(end.X - normal.X * 6f, end.Y - normal.Y * 6f)
    };
    graphics.FillPolygon(Brushes.White, head);
  }

  [DllImport("user32.dll", CharSet = CharSet.Unicode)]
  private static extern uint PrivateExtractIcons(
    string fileName,
    int iconIndex,
    int width,
    int height,
    IntPtr[] icons,
    uint[] iconIds,
    uint count,
    uint flags);

  [DllImport("user32.dll")]
  private static extern bool DestroyIcon(IntPtr handle);
}
