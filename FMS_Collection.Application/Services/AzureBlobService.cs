using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Configuration;

public class AzureBlobService
{
    private readonly string _connectionString;
    private readonly string _containerName;
    public AzureBlobService(IConfiguration config)
    {
        _connectionString = config["AzureStorage:ConnectionString"];
        _containerName = config["AzureStorage:ContainerName"];
    }
    public string GetBlobSasUrl(string blobName, int validHours = 24)
    {
        var blobServiceClient = new BlobServiceClient(_connectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);
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
}
