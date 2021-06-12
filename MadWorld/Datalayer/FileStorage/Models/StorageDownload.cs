using System;
using System.IO;

namespace Datalayer.FileStorage.Models
{
    public class StorageDownload
    {
        public bool Succeed { get; set; }
        public MemoryStream Stream { get; set; }
    }
}
