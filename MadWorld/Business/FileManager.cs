using System;
using System.Collections.Generic;
using System.Linq;
using Business.Interfaces;
using Datalayer.Database.Queries.Interfaces;
using Datalayer.Database.Tables;
using Datalayer.FileStorage.Interfaces;
using Website.Shared.Models;

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

        public List<FileEditItem> GetFiles()
        {
            List<FileInfo> fileInfos = _fileQueries.GetAll();

            if (fileInfos == null) return new List<FileEditItem>();

            return fileInfos.Select(fi => new FileEditItem {
                ID = fi.ID,
                Name = fi.Name,
                Type = fi.Type
            }).ToList();
        }
    }
}
