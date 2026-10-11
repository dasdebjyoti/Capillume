using System.Drawing;
using Capillume;
using Xunit;

namespace Capillume.Tests;

public sealed class ScreenshotDedupeServiceTests
{
    [Fact]
    public void Compare_IdenticalImages_SkipsDuplicate()
    {
        using var service = new ScreenshotDedupeService();
        using var first = CreateBitmap(Color.White);
        using var second = CreateBitmap(Color.White);
        DedupeSettings settings = CreateSettings();

        ScreenshotDedupeResult initial = service.Compare(Request(first), settings, CancellationToken.None);
        service.Commit(initial);
        ScreenshotDedupeResult result = service.Compare(Request(second), settings, CancellationToken.None);

        Assert.Equal(ScreenshotDedupeDecision.SkipDuplicate, result.Decision);
    }

    [Fact]
    public void Compare_ExcludedDifference_SkipsDuplicate()
    {
        using var service = new ScreenshotDedupeService();
        using var first = CreateBitmap(Color.White);
        using var second = CreateBitmap(Color.White);
        using (Graphics graphics = Graphics.FromImage(second))
        {
            graphics.FillRectangle(Brushes.Black, new Rectangle(0, 0, 30, 30));
        }

        DedupeSettings settings = CreateSettings();
        ScreenshotDedupeResult initial = service.Compare(Request(first, [new Rectangle(0, 0, 30, 30)]), settings, CancellationToken.None);
        service.Commit(initial);
        ScreenshotDedupeResult result = service.Compare(Request(second, [new Rectangle(0, 0, 30, 30)]), settings, CancellationToken.None);

        Assert.Equal(ScreenshotDedupeDecision.SkipDuplicate, result.Decision);
    }

    [Fact]
    public void Compare_LargeContentChange_Saves()
    {
        using var service = new ScreenshotDedupeService();
        using var first = CreateBitmap(Color.White);
        using var second = CreateBitmap(Color.Black);
        DedupeSettings settings = CreateSettings();

        ScreenshotDedupeResult initial = service.Compare(Request(first), settings, CancellationToken.None);
        service.Commit(initial);
        ScreenshotDedupeResult result = service.Compare(Request(second), settings, CancellationToken.None);

        Assert.Equal(ScreenshotDedupeDecision.Save, result.Decision);
        Assert.True(result.SsimScore < 0.995);
    }

    private static DedupeSettings CreateSettings() => new()
    {
        Enabled = true,
        CompareMode = DedupeCompareMode.SsimOnly,
        Sensitivity = 50
    };

    private static ScreenshotDedupeRequest Request(Bitmap bitmap, IReadOnlyCollection<Rectangle>? exclusions = null) => new()
    {
        Bitmap = bitmap,
        BaselineKey = "test",
        ComparisonBounds = new Rectangle(Point.Empty, bitmap.Size),
        ExcludedRegions = exclusions ?? Array.Empty<Rectangle>()
    };

    private static Bitmap CreateBitmap(Color color)
    {
        var bitmap = new Bitmap(120, 80);
        using Graphics graphics = Graphics.FromImage(bitmap);
        graphics.Clear(color);
        return bitmap;
    }
}
