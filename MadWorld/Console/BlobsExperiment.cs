using System;
using System.IO;
using System.Text;
using Azure.Storage.Blobs;

namespace Console
{
    public class BlobsExperiment
    {
        public void Start()
        {
            string connectionString = "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";
            string containerName = "test";
            string fileName = "Random";

            BlobContainerClient container = new BlobContainerClient(connectionString, containerName);

            container.CreateIfNotExists();

            BlobClient blobUpload = container.GetBlobClient(fileName);
            blobUpload.DeleteIfExists();

            string testContent = "Hello!";
            byte[] content = Encoding.UTF8.GetBytes(testContent);
            using (var ms = new MemoryStream(content))
            {
                blobUpload.Upload(ms);
            }

            BlobClient blobDownload = container.GetBlobClient(fileName);
            var downloadContentResponse = blobDownload.DownloadStreaming();
            var downloadContent = downloadContentResponse.Value.Content;
            var bytesContent = new byte[0];

            using (var ms = new MemoryStream())
            {
                downloadContent.CopyTo(ms);
                bytesContent = ms.ToArray();
            }

            var ultimateResult = Encoding.UTF8.GetString(bytesContent);
        }
    }
}
