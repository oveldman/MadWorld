using System;
using System.Collections.Generic;
using System.IO;
using Datalayer.FileStorage.Interfaces;
using Datalayer.FileStorage.Models;

namespace Datalayer.FileStorage
{
    public class StorageContainer : IStorageContainer
    {
        private readonly IDiskManager _diskManager;
        private readonly StorageSettings _settings;
        private readonly string ContainerName;
        private string ContainerPath
        {
            get {
                return Path.Combine(_settings.BasePath, ContainerName);
            }
        }

        public StorageContainer(IDiskManager diskManager, StorageSettings settings, string containerName)
        {
            _diskManager = diskManager;
            _settings = settings;
            ContainerName = containerName;
        }

        public StorageResult Create()
        {
            try
            {
                _diskManager.CreateDirectory(ContainerPath);
            }
            catch (Exception ex)
            {
                return new StorageResult
                {
                    Succeed = false,
                    ErrorMessages = new List<string> {
                        ex.Message
                    }
                };
            }

            return new StorageResult
            {
                Succeed = true
            };
        }

        public StorageResult CreateIfNotExists()
        {
            if(!_diskManager.DirectoryExists(ContainerPath))
            {
                return Create();
            }

            return new StorageResult
            {
                Succeed = true
            };
        }

        public IStorageFile GetFile(string name, string path = "")
        {
            return new StorageFile(_diskManager, _settings, ContainerPath, path, name);
        }
    }
}
