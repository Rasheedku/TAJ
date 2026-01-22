using System.Text.Json;
using Windows.Storage;

namespace Taj.BuildingCostApp.Services;

public class JsonStorageService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public async Task<T> LoadAsync<T>(string fileName, Func<T> defaultFactory)
    {
        var file = await EnsureFileAsync(fileName, defaultFactory);
        try
        {
            await using var stream = await file.OpenStreamForReadAsync();
            var result = await JsonSerializer.DeserializeAsync<T>(stream, JsonOptions);
            return result ?? defaultFactory();
        }
        catch
        {
            var fallback = defaultFactory();
            await SaveAsync(fileName, fallback);
            return fallback;
        }
    }

    public async Task SaveAsync<T>(string fileName, T data)
    {
        var localFolder = ApplicationData.Current.LocalFolder;
        var file = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
        await using var stream = await file.OpenStreamForWriteAsync();
        await JsonSerializer.SerializeAsync(stream, data, JsonOptions);
    }

    private async Task<StorageFile> EnsureFileAsync<T>(string fileName, Func<T> defaultFactory)
    {
        var localFolder = ApplicationData.Current.LocalFolder;
        var localFile = await localFolder.TryGetItemAsync(fileName) as StorageFile;
        if (localFile != null)
        {
            return localFile;
        }

        var packagedPath = Path.Combine(AppContext.BaseDirectory, "Data", fileName);
        if (File.Exists(packagedPath))
        {
            var content = await File.ReadAllTextAsync(packagedPath);
            localFile = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(localFile, content);
            return localFile;
        }

        var data = defaultFactory();
        await SaveAsync(fileName, data);
        return await localFolder.GetFileAsync(fileName);
    }
}
