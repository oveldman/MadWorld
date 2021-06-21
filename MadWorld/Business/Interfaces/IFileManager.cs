using System;
using System.Collections.Generic;
using Website.Shared.Models;

namespace Business.Interfaces
{
    public interface IFileManager
    {
        List<FileEditItem> GetFiles();
    }
}
