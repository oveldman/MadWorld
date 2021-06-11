using System;
using System.ComponentModel.DataAnnotations;

namespace Datalayer.Database.Tables
{
    public class Log
    {
        [Key]
        public Guid ID { get; set; }
        public string Application { get; set; }
        public string Text { get; set; }
        public int Level { get; set; }
        public string Message { get; set; }
        public string Logger { get; set; }
        public string StackTrace { get; set; }
        public string Exception { get; set; }
        public DateTime Created { get; set; }
    }
}
