using System;
using System.ComponentModel.DataAnnotations;
using Datalayer.Database.Models;

namespace Datalayer.Database.Tables
{
    public class FileInfo
    {
        [Key]
        public Guid ID { get; set; }
        public DateTime Created { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string Type { get; set; }
        public FileType AccessType { get; set; }
    }
}
