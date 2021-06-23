using System;
using System.Collections.Generic;
using System.Linq;
using Business.Interfaces;
using Common;
using Datalayer.Database.Models;
using Datalayer.Database.Queries.Interfaces;
using Datalayer.Database.Tables;
using Datalayer.FileStorage.Interfaces;
using Datalayer.FileStorage.Models;
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

        public BaseModel CreateFile(Guid? id, string name, string type, string bodyBase64)
        {
            bool succeed = false;

            if (!id.HasValue)
            {
                id = Guid.NewGuid();
            }

            FileInfo fileInfo = new()
            {
                ID = id.Value,
                AccessType = FileType.Anonymous,
                Created = SystemTime.Now(),
                Name = name,
                Type = type,
                Extension = System.IO.Path.GetExtension(name),
                Show = true
            };

            DbResult dbResult = _fileQueries.Add(fileInfo);

            if (dbResult.Succeed)
            {
                StorageResult storageResult = _storageManager.Upload(StoragePaths.FreeFiles, fileInfo.FullStorageName, bodyBase64);

                succeed = storageResult.Succeed;
            }

            return new BaseModel
            {
                Succeed = succeed,
                ErrorMessage = succeed ? string.Empty : "Failed to save file"
            };
        }

        public BaseModel DeleteFile(Guid id)
        {
            bool succeed = false;

            FileInfo fileInfo = _fileQueries.Get(id);

            if (fileInfo is not null)
            {
                StorageResult storageResult = _storageManager.Delete(StoragePaths.FreeFiles, fileInfo.FullStorageName);

                if (storageResult.Succeed)
                {
                    DbResult dbResult = _fileQueries.Delete(id);
                    succeed = dbResult.Succeed;
                }
            }

            return new BaseModel
            {
                Succeed = succeed,
                Message = succeed ? string.Empty : "Delete file failed"
            };
        }

        public FileItem GetFile(Guid id, FileType fileType)
        {
            FileInfo fileInfo = _fileQueries.Get(id, fileType);

            if (fileInfo is not null)
            {
                string body = _storageManager.DownloadString(StoragePaths.FreeFiles, fileInfo.FullStorageName);

                return new FileItem
                {
                    BodyBase64 = body,
                    Name = fileInfo.Name,
                    Type = fileInfo.Type
                };
            }

            return null;
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
