using Azure.Storage.Blobs;

namespace WoofAdopciones.Backend.Helpers
{
    public class BlobContainerClientFactory : IBlobContainerClientFactory
    {
        public IBlobContainerClient CreateBlobContainerClient(string connectionString, string containerName) => new BlobContainerClientWrapper(connectionString, containerName);
    }
}