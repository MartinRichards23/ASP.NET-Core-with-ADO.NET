using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using SystemPlus.IO;

namespace WebsiteTemplate.Data.Storage
{
    public static class BlobExstensions
    {
        /// <summary>
        /// Downloads text, decompressing if necessary
        /// </summary>
        public async static Task<string> DownloadCompressedTextAsync(this CloudBlockBlob blob)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                await blob.DownloadToStreamAsync(ms);
                byte[] data = ms.ToArray();

                if (blob.Properties.ContentEncoding == "gzip")
                {
                    // decompress data
                    data = Compression.Decompress(data);
                }

                return Encoding.UTF8.GetString(data);
            }
        }

        /// <summary>
        /// Uploads text, compressing it first
        /// Sets content encoding
        /// </summary>
        public async static Task UploadCompressedTextAsync(this CloudBlockBlob blob, string text)
        {
            blob.Properties.ContentEncoding = "gzip";

            byte[] data = Encoding.UTF8.GetBytes(text);
            await blob.UploadFromByteArrayAsync(data, 0, data.Length);
        }

        public async static Task<List<IListBlobItem>> ListBlobsAsync(this CloudBlobContainer container)
        {
            BlobContinuationToken continuationToken = null;
            List<IListBlobItem> results = new List<IListBlobItem>();

            do
            {
                var response = await container.ListBlobsSegmentedAsync(continuationToken);
                continuationToken = response.ContinuationToken;
                results.AddRange(response.Results);
            }
            while (continuationToken != null);

            return results;
        }

    }
}