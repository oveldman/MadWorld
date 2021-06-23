using System;
using System.Collections.Generic;
using Website.Shared.Models;

namespace Business.Interfaces
{
    public interface IFileManager
    {
        BaseModel CreateFile(Guid? id, string name, string type, string bodyBase64);
        BaseModel DeleteFile(Guid id);
        List<FileEditItem> GetFiles();
    }
}
