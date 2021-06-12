using System;
using Datalayer.FileStorage.Interfaces;
using Datalayer.FileStorage.Models;

namespace Datalayer.FileStorage
{
    public class StorageExplorer : IStorageExplorer
    {
        private IDiskManager _diskManager;
        private StorageSettings _settings;

        public StorageExplorer(IDiskManager diskManager, StorageSettings settings)
        {
            _diskManager = diskManager;
            _settings = settings;
        }

        public IStorageContainer GetContainer(string name)
        {
            return new StorageContainer(_diskManager, _settings, name);
        }
    }
}
