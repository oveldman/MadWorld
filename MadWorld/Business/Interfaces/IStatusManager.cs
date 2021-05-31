using System;
namespace Business.Interfaces
{
    public interface IStatusManager
    {
        public bool IsDatabaseMadWorldOnline();
        public bool IsDatabaseAuthenticationOnline();
    }
}
