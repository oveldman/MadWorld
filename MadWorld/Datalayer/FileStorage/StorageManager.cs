using System;
using System.IO;
using System.Text;
using Datalayer.FileStorage.Interfaces;
using Datalayer.FileStorage.Models;

namespace Datalayer.FileStorage
{
    public class StorageManager : IStorageManager
    {
        private IStorageContainer _container;

        public StorageManager(IStorageExplorer explorer, StorageSettings settings)
        {
            _container = explorer.GetContainer(settings.StandardContainer);
            _container.CreateIfNotExists();
        }

        public byte[] DownloadBytes(string path, string filename)
        {
            MemoryStream stream = DownloadStream(path, filename);
            return stream.ToArray();
        }

        public MemoryStream DownloadStream(string path, string filename)
        {
            IStorageFile file = _container.GetFile(filename, path);
            StorageDownload result = file.Download();
            return result.Stream;
        }

        public string DownloadString(string path, string filename)
        {
            byte[] body = DownloadBytes(path, filename);
            return Encoding.UTF8.GetString(body);
        }

        public StorageResult Upload(string path, string filename, byte[] file)
        {
            MemoryStream stream = new(file);
            return Upload(path, filename, stream);
        }

        public StorageResult Upload(string path, string filename, MemoryStream file)
        {
            IStorageFile storageFile = _container.GetFile(filename, path);
            storageFile.DeleteIfExists();
            return storageFile.Upload(file);
        }

        public StorageResult Upload(string path, string filename, string file)
        {
            byte[] body = Encoding.ASCII.GetBytes(file);
            return Upload(path, filename, body);
        }
    }
}
