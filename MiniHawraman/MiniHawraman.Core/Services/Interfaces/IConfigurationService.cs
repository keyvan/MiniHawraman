using MiniHawraman.Core.Models;
namespace MiniHawraman.Core.Services.Interfaces
{
    public interface IConfigurationService
    {
        Configuration GetConfiguration(int id);
        Configuration GetConfiguration(string name);
        void AddConfiguration(Configuration configuration);
        void EditConfiguration(Configuration configuration);
        void DeleteConfiguration(int id);
    }
}
