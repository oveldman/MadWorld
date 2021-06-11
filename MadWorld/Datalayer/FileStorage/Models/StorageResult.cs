using System;
using System.Collections.Generic;

namespace Datalayer.FileStorage.Models
{
    public class StorageResult
    {
        public bool Succeed { get; set; }
        public List<string> ErrorMessages { get; set; } = new();
    }
}
