using System;
using Database.Tables;

namespace Business.Interfaces
{
    public interface IResumeManager
    {
        Resume GetLastResume(); 
    }
}
