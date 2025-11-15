using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using FMS_Collection.Core.Common;
using Microsoft.Extensions.Configuration;
using System.IO.Compression;

public class AzureBlobService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;
    private readonly string _accountName;
    private readonly string _accountKey;

    public AzureBlobService(IConfiguration configuration)
    {
        var connectionString = AppSettings.AzureStorageConnectionString;
        _containerName = AppSettings.AzureStorageContainerName;
        _accountName = configuration["AzureStorage:AccountName"];
        _accountKey = configuration["AzureStorage:AccountKey"];

        _blobServiceClient = new BlobServiceClient(connectionString);
    }

    public string GetBlobSasUrl(string blobName, int validHours = 24)
    {

        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(blobName);

        if (!blobClient.CanGenerateSasUri)
            throw new InvalidOperationException("SAS not supported for this client.");

        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = _containerName,
            BlobName = blobName,
            Resource = "b",
            ExpiresOn = DateTimeOffset.UtcNow.AddHours(validHours) // Valid for 24 hours
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        var sasUri = blobClient.GenerateSasUri(sasBuilder);
        return sasUri.ToString();
    }

    public async Task UploadFileAsync(byte[] fileBytes, string blobPath)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync();

        var blobClient = containerClient.GetBlobClient(blobPath);

        using var stream = new MemoryStream(fileBytes);
        await blobClient.UploadAsync(stream, overwrite: true);
    }
    public async Task<bool> DeleteFileAsync(string blobPath)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(blobPath);
        return await blobClient.DeleteIfExistsAsync();
    }

    public async Task<byte[]> DownloadFolderAsZipAsync(string containerName, string folderPath)
    {
        string connectionString = "<YourConnectionString>";

        BlobContainerClient container = new BlobContainerClient(connectionString, containerName);

        using MemoryStream zipStream = new MemoryStream();

        using (ZipArchive zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
        {
            await foreach (var blobItem in container.GetBlobsAsync(prefix: folderPath))
            {
                var blobClient = container.GetBlobClient(blobItem.Name);

                using var blobData = new MemoryStream();
                await blobClient.DownloadToAsync(blobData);
                blobData.Position = 0;

                var entry = zipArchive.CreateEntry(blobItem.Name.Replace(folderPath, ""));
                using var entryStream = entry.Open();
                blobData.CopyTo(entryStream);
            }
        }

        return zipStream.ToArray(); // return ZIP bytes
    }
}
