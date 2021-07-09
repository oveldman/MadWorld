using System;
using System.ComponentModel.DataAnnotations;

namespace Datalayer.Database.Tables
{
    public class Post
    {
        [Key]
        public Guid ID { get; set; }
        public DateTime Created { get; set; }
        public Guid FileID { get; set; }
        public string Title { get; set; }
        public Guid WriterID { get; set; }
    }
}
