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
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=https://127.0.0.1:10000/devstoreaccount1;";
            string containerName = "Test";
            string fileName = "Random";

            BlobContainerClient container = new BlobContainerClient(connectionString, containerName);
            container.Create();

            BlobClient blobUpload = container.GetBlobClient(fileName);

            string testContent = "Hello!";
            byte[] content = Encoding.UTF8.GetBytes(testContent);
            using (var ms = new MemoryStream(content))
            {
                blobUpload.Upload(ms);
            }

            BlobClient blobDownload = container.GetBlobClient(fileName);
            var downloadContentResponse = blobDownload.DownloadContent();
            var downloadContent = downloadContentResponse.GetRawResponse();
            string ultimateContentTest = downloadContent.ToString();
        }
    }
}
