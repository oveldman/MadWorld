using System;
namespace Website.Shared.Models.Account
{
    public class TwoFactorRequest
    {
        public Guid? Session { get; set; }
        public string Token { get; set; }
    }
}
