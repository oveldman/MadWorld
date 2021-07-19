using System;
namespace Website.Shared.Models.Admin
{
    public class AdminPostModel : BaseModel
    {
        public string ID { get; set; }
        public DateTime Created { get; set; }
        public string Title { get; set; }
    }
}
