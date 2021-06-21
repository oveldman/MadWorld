using System;
using System.Collections.Generic;

namespace Website.Shared.Models
{
    public class FilesResponse : BaseModel
    {
        public List<FileItem> Files { get; set; }
    }
}
