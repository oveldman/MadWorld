using System;
using System.ComponentModel.DataAnnotations;

namespace Datalayer.Database.Tables
{
    public class Resume
    {
        [Key]
        public Guid ID { get; set; }
        public string FullName { get; set; }
        public DateTime? Created { get; set; }
    }
}
