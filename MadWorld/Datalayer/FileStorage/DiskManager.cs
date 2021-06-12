using System;
using System.IO;
using Datalayer.FileStorage.Interfaces;

namespace Datalayer.FileStorage
{
    public class DiskManager : IDiskManager
    {
        public DirectoryInfo CreateDirectory(string path)
        {
            return Directory.CreateDirectory(path);
        }

        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public FileStream FileCreate(string filePath)
        {
            return File.Create(filePath);
        }

        public void FileDelete(string filePath)
        {
            File.Delete(filePath);
        }

        public bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public FileStream FileRead(string filePath)
        {
            return new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
        }
    }
}
