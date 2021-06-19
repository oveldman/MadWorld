using System;
using System.IO;
using Datalayer.FileStorage.Models;

namespace Datalayer.FileStorage.Interfaces
{
    public interface IStorageManager
    {
        byte[] DownloadBytes(string path, string filename);
        MemoryStream DownloadStream(string path, string filename);
        string DownloadString(string path, string filename);
        T DownloadJsonClass<T>(string path, string filename);
        StorageResult Upload(string path, string filename, byte[] file);
        StorageResult Upload(string path, string filename, MemoryStream file);
        StorageResult Upload(string path, string filename, string file);
        StorageResult Upload<T>(string path, string filename, T objectToSave);
        StorageResult Delete(string path, string filename);
    }
}
