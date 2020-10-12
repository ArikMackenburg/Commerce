using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Web.Services
{
    public interface IUploadServices
    {
        Task<string> UploadFile(string filename, Stream data);
    }

    public class AzureUploadService : IUploadServices
    {
        public AzureUploadService(IConfiguration configuration)
        {
            var accountName = configuration["AzureStorageAccountName"];
            var blobKey = configuration["AzureBlobKey"];

            var storageCredentials = new StorageCredentials(accountName, blobKey);
            var storageAccount = new CloudStorageAccount(storageCredentials, true);

            CloudBlobClient = storageAccount.CreateCloudBlobClient();
        }

        public CloudBlobClient CloudBlobClient { get; }

        public async Task<string> UploadFile(string filename, Stream data)
        {
            var container = CloudBlobClient.GetContainerReference("images");
            await container.CreateIfNotExistsAsync();
            await container.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob,
            });

            var blobFile = container.GetBlockBlobReference(filename);
            await blobFile.UploadFromStreamAsync(data);
            return blobFile.Uri.ToString();
        }
    }
}
