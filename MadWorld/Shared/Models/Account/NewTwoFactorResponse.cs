using System;
namespace Website.Shared.Models.Account
{
    public class NewTwoFactorResponse : BaseModel
    {
        public bool IsTwoFactorOn { get; set; }
        public string Secret { get; set; }
        public string QRBase64 { get; set; }
        public string ApplicationName { get; set; }
    }
}
