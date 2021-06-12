using System;
using System.IO;

namespace Datalayer.FileStorage.Interfaces
{
    public interface IDiskManager
    {
        DirectoryInfo CreateDirectory(string path);
        bool DirectoryExists(string path);
        FileStream FileCreate(string filePath);
        void FileDelete(string filePath);
        bool FileExists(string filePath);
        FileStream FileRead(string filePath);
    }
}
