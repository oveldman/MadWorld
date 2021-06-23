using System;
using System.Collections.Generic;
using Datalayer.Database.Models;
using Datalayer.Database.Tables;

namespace Datalayer.Database.Queries.Interfaces
{
    public interface IFileQueries
    {
        DbResult Add(FileInfo file);
        DbResult Delete(Guid id);
        FileInfo Get(Guid id);
        FileInfo Get(Guid id, FileType accessType);
        List<FileInfo> GetAll();
    }
}
