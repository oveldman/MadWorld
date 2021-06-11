using System;
using Datalayer.FileStorage.Interfaces;
using Datalayer.FileStorage.Models;

namespace Datalayer.FileStorage
{
    public class StorageExplorer : IStorageExplorer
    {
        private StorageSettings _settings { get; set; }

        public StorageExplorer(StorageSettings settings)
        {
            _settings = settings;
        }

        public IStorageContainer GetContainer(string name)
        {
            return new StorageContainer(_settings, name);
        }
    }
}
