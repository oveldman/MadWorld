using System;
using Datalayer.FileStorage.Models;

namespace Datalayer.FileStorage.Interfaces
{
    public interface IStorageContainer
    {
        StorageResult Create();
        StorageResult CreateIfNotExists();
        IStorageFile GetFile(string name, string path = "");
    }
}
