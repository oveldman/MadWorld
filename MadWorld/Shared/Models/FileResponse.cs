using System;
namespace Website.Shared.Models
{
    public class FileResponse : BaseModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string BodyBase64 { get; set; }
    }
}
