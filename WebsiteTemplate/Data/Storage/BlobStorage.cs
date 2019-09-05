using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace WebsiteTemplate.Data.Storage
{
    /// <summary>
    /// Manages access to the blob store
    /// </summary>
    public class BlobStorage
    {
        public BlobStorage(string accountName, string accountKey)
        {
            StorageCredentials creds = new StorageCredentials(accountName, accountKey);
            StorageAccount = new CloudStorageAccount(creds, true);
        }

        public CloudStorageAccount StorageAccount { get; }

        public CloudBlobContainer GetBlobContainer(string containerName)
        {
            CloudBlobClient blobClient = StorageAccount.CreateCloudBlobClient();
            return blobClient.GetContainerReference(containerName);
        }

        public async Task<string> GetString(string containerName, string name)
        {
            CloudBlobContainer container = GetBlobContainer(containerName);
            CloudBlockBlob blob = container.GetBlockBlobReference(name);
            return await blob.DownloadCompressedTextAsync();
        }

        public async Task<Stream> GetStream(string containerName, string name)
        {
            CloudBlobContainer container = GetBlobContainer(containerName);
            CloudBlockBlob blob = container.GetBlockBlobReference(name);            
            return await blob.OpenReadAsync();
        }

        public async Task<byte[]> GetBytes(string containerName, string name)
        {
            CloudBlobContainer container = GetBlobContainer(containerName);
            CloudBlockBlob blob = container.GetBlockBlobReference(name);

            using (MemoryStream ms = new MemoryStream())
            {
                await blob.DownloadToStreamAsync(ms);
                return ms.ToArray();
            }
        }

        public async Task<bool> DeleteBlob(string containerName, string name)
        {
            CloudBlobContainer container = GetBlobContainer(containerName);
            CloudBlockBlob blob = container.GetBlockBlobReference(name);
            return await blob.DeleteIfExistsAsync();
        }

        public async Task SetCacheControl()
        {
            CloudBlobContainer container = GetBlobContainer("public");

            TimeSpan ts = TimeSpan.FromDays(90);

            string cacheControl = string.Format("max-age={0}, must-revalidate", ts.TotalSeconds);

            foreach (IListBlobItem item in await container.ListBlobsAsync())
            {
                CloudBlockBlob blob = item as CloudBlockBlob;

                if (blob != null)
                {
                    blob.Properties.CacheControl = cacheControl;
                    await blob.SetPropertiesAsync();
                }
            }
        }

    }
}
