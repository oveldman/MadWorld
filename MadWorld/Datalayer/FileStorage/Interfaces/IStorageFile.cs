using System;
using System.IO;
using Datalayer.FileStorage.Models;

namespace Datalayer.FileStorage.Interfaces
{
    public interface IStorageFile
    {
        StorageResult DeleteIfExists();
        StorageResult Upload(MemoryStream stream);
        StorageDownload Download();
    }
}
