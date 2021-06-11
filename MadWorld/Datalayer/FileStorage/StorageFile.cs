using System;
using System.IO;
using Datalayer.FileStorage.Interfaces;
using Datalayer.FileStorage.Models;

namespace Datalayer.FileStorage
{
    public class StorageFile : IStorageFile
    {
        private readonly StorageSettings _settings;
        private readonly string ContainerPath;
        private readonly string Filename;
        private string FullPath
        {
            get
            {
                return Path.Combine(ContainerPath, Filename);
            }
        }

        public StorageFile(StorageSettings settings, string containerPath, string filename)
        {
            _settings = settings;
            ContainerPath = containerPath;
            Filename = filename;
        }

        public StorageResult DeleteIfExists()
        {
            if(File.Exists(FullPath)) {
                File.Delete(FullPath);
            }

            return new StorageResult
            {
                Succeed = true
            };
        }

        public StorageDownload Download()
        {
            FileStream fileStream = new FileStream(FullPath, FileMode.Open, FileAccess.Read, FileShare.None);

            return new StorageDownload
            {
                Stream = fileStream
            };
        }

        public StorageResult Upload(MemoryStream fileStream)
        {
            using (var stream = System.IO.File.Create(FullPath))
            {
                fileStream.CopyTo(stream);
            }

            return new StorageResult
            {
                Succeed = true
            };
        }
    }
}
