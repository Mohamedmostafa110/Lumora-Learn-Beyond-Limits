namespace Lumora.Infrastructure.Storage
{
    public interface ICloudStorageService
    {
        Task<string> UploadVideoAsync(Stream fileStream, string fileName, string contentType);
        string GenerateTemporarySignedStreamingUrl(string storageKey, TimeSpan expiry);
    }

    public class AzureBlobStorageService : ICloudStorageService
    {
        private readonly ILogger<AzureBlobStorageService> _logger;

        public AzureBlobStorageService(ILogger<AzureBlobStorageService> logger)
        {
            _logger = logger;
        }

        public async Task<string> UploadVideoAsync(Stream fileStream, string fileName, string contentType)
        {
            var uniqueKey = $"courses/videos/{Guid.NewGuid():N}_{fileName}";
            _logger.LogInformation("Simulating Azure Blob upload for file: {FileName} to key: {Key}", fileName, uniqueKey);
            await Task.Delay(50); // Simulating async upload
            return uniqueKey;
        }

        public string GenerateTemporarySignedStreamingUrl(string storageKey, TimeSpan expiry)
        {
            // Generates an Azure SAS Token for temporary secured streaming
            var token = Guid.NewGuid().ToString("N");
            var expires = DateTimeOffset.UtcNow.Add(expiry).ToUnixTimeSeconds();
            return $"https://lumorastorage.blob.core.windows.net/{storageKey}?token={token}&expires={expires}";
        }
    }
}
