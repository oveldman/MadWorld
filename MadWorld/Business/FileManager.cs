using System;
using Business.Interfaces;
using Datalayer.Database.Queries.Interfaces;
using Datalayer.FileStorage.Interfaces;

namespace Business
{
    public class FileManager : IFileManager
    {
        private IFileQueries _fileQueries;
        private IStorageManager _storageManager;

        public FileManager(IFileQueries fileQueries, IStorageManager storageManager)
        {
            _fileQueries = fileQueries;
            _storageManager = storageManager;
        }
    }
}
