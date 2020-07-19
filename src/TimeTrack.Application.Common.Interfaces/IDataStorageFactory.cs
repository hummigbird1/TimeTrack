using Microsoft.Extensions.Configuration;
using TimeTrack.Interfaces;

namespace TimeTrack.Application.Common.Interfaces
{
    public interface IDataStorageFactory
    {
        IDataStorage CreateDataStorageInstance(IConfiguration configuration);
    }
}