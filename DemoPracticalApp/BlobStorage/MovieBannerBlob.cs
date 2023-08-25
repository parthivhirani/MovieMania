using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using Azure.Storage.Blobs;

namespace DemoPracticalApp.BlobStorage
{
    public class MovieBannerBlob
    {
        private readonly CloudBlobContainer _blobContainer;

        public MovieBannerBlob(IConfiguration configuration)
        {
            var storageAccount = CloudStorageAccount.Parse(configuration.GetConnectionString("AzureStorageConnection"));
            var blobClient = storageAccount.CreateCloudBlobClient();
            _blobContainer = blobClient.GetContainerReference("moviebanners");
        }

        public async Task<string> storeAsBlob(IFormFile photo)
        {
            if (photo == null || photo.Length == 0)
            {
                throw new ArgumentException("Photo is invalid.");
            }

            var blobName = Guid.NewGuid().ToString() + "_" + photo.FileName;
            var blob = _blobContainer.GetBlockBlobReference(blobName);

            using (var stream = photo.OpenReadStream())
            {
                await blob.UploadFromStreamAsync(stream);
            }

            return blob.Uri.ToString();
        }

        public async Task deleteFromBlob(string url)
        {
            var blob = await _blobContainer.ServiceClient.GetBlobReferenceFromServerAsync(new Uri(url));
            await blob.DeleteIfExistsAsync();
        }
    }
}
