using System;
using Datalayer.Database.Tables;

namespace Business.Interfaces
{
    public interface IResumeManager
    {
        Resume GetLastResume(); 
    }
}
