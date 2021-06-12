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
        private readonly string FilePath;
        private string FullPathOnly
        {
            get
            {
                return Path.Combine(ContainerPath, FilePath);
            }
        }
        private string FullPath
        {
            get
            {
                return Path.Combine(ContainerPath, FilePath, Filename);
            }
        }

        public StorageFile(StorageSettings settings, string containerPath, string path, string filename)
        {
            _settings = settings;
            ContainerPath = containerPath;
            Filename = filename;
            FilePath = path;
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
            FileStream fileStream = new(FullPath, FileMode.Open, FileAccess.Read, FileShare.None);

            using (MemoryStream stream = new MemoryStream())
            {
                fileStream.CopyTo(stream);

                return new StorageDownload
                {
                    Stream = stream
                };
            }
        }

        public StorageResult Upload(MemoryStream fileStream)
        {
            Directory.CreateDirectory(FullPathOnly);

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
