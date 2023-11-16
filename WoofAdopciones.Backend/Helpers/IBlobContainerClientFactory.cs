using Azure.Storage.Blobs;

namespace WoofAdopciones.Backend.Helpers
{
    public interface IBlobContainerClientFactory
    {
        IBlobContainerClient CreateBlobContainerClient(string connectionString, string containerName);
    }
}