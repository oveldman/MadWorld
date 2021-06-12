using System;
using Datalayer.FileStorage;
using Datalayer.FileStorage.Interfaces;
using Datalayer.FileStorage.Models;

namespace Console
{
    public class StorageExample
    {
        public static void Start()
        {
            StorageSettings settings = new()
            {
                StandardContainer = "Random",
                BasePath = "Test"
            };

            IDiskManager diskManager = new DiskManager();
            IStorageExplorer explorer = new StorageExplorer(diskManager, settings);
            IStorageManager manager = new StorageManager(explorer, settings);
            manager.Upload("txt", "Test.txt", "Hello World!");
            string test = manager.DownloadString("", "Test.txt");
            System.Console.WriteLine(test);
        }
    }
}
