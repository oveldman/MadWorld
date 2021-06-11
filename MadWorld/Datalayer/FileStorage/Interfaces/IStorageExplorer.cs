using System;
namespace Datalayer.FileStorage.Interfaces
{
    public interface IStorageExplorer
    {
        IStorageContainer GetContainer(string name);
    }
}
