
namespace MiniHawraman.Core.Services.Interfaces
{
    public interface IMembershipService
    {
        bool ValidateUser(string userName, string password);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
    }
}
