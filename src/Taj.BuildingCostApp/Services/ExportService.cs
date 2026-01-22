using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Imaging;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Taj.BuildingCostApp.ViewModels;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

namespace Taj.BuildingCostApp.Services;

public class ExportService
{
    static ExportService()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public async Task<string?> ExportPdfAsync(CalculatorSummary summary)
    {
        var file = await PickSaveFileAsync("TAJ-Summary.pdf", ".pdf");
        if (file == null)
        {
            return null;
        }

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);
                page.Content().Column(column =>
                {
                    column.Item().Text("TAJ - Project Cost Summary").FontSize(20).SemiBold();
                    column.Item().Text($"Date: {DateTime.Now:yyyy-MM-dd HH:mm}");
                    column.Item().PaddingTop(10);
                    foreach (var line in summary.Lines)
                    {
                        column.Item().Text($"{line.Label}: {line.Value:N2} AED");
                    }
                    column.Item().PaddingTop(10);
                    column.Item().Text($"Grand Total: {summary.GrandTotal:N2} AED").FontSize(18).SemiBold();
                });
            });
        });

        await using var stream = await file.OpenStreamForWriteAsync();
        document.GeneratePdf(stream);
        return file.Path;
    }

    public async Task<string?> ExportPngAsync(UIElement element)
    {
        var file = await PickSaveFileAsync("TAJ-Summary.png", ".png");
        if (file == null)
        {
            return null;
        }

        var rtb = new RenderTargetBitmap();
        await rtb.RenderAsync(element);
        var pixels = await rtb.GetPixelsAsync();

        await using var stream = await file.OpenAsync(FileAccessMode.ReadWrite);
        var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
        encoder.SetPixelData(
            BitmapPixelFormat.Bgra8,
            BitmapAlphaMode.Premultiplied,
            (uint)rtb.PixelWidth,
            (uint)rtb.PixelHeight,
            96,
            96,
            pixels.ToArray());
        await encoder.FlushAsync();
        return file.Path;
    }

    private static async Task<StorageFile?> PickSaveFileAsync(string suggestedFileName, string extension)
    {
        var picker = new FileSavePicker
        {
            SuggestedFileName = suggestedFileName
        };
        picker.FileTypeChoices.Add("Export", new List<string> { extension });

        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindowInstance);
        WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
        return await picker.PickSaveFileAsync();
    }
}
