using System;
using System.IO;
using Datalayer.FileStorage.Interfaces;
using Datalayer.FileStorage.Models;

namespace Datalayer.FileStorage
{
    public class StorageFile : IStorageFile
    {
        private readonly IDiskManager _diskManager;
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

        public StorageFile(IDiskManager diskManager, StorageSettings settings, string containerPath, string path, string filename)
        {
            _diskManager = diskManager;
            _settings = settings;
            ContainerPath = containerPath;
            Filename = filename;
            FilePath = path;
        }

        public StorageResult DeleteIfExists()
        {
            if(_diskManager.FileExists(FullPath)) {
                _diskManager.FileDelete(FullPath);
            }

            return new StorageResult
            {
                Succeed = true
            };
        }

        public StorageDownload Download()
        {
            using (FileStream fileStream = _diskManager.FileRead(FullPath)) {
                using (MemoryStream stream = new MemoryStream())
                {
                    fileStream.CopyTo(stream);

                    return new StorageDownload
                    {
                        Stream = stream
                    };
                }
            }
        }

        public StorageResult Upload(MemoryStream fileStream)
        {
            _diskManager.CreateDirectory(FullPathOnly);

            using (var stream = _diskManager.FileCreate(FullPath))
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
