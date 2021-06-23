using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public bool Show { get; set; }

        [NotMapped]
        public string FullStorageName
        {
            get
            {
                return ID.ToString() + Extension;
            }
        }
    }
}
