
using System.Collections.Generic;
using MiniHawraman.Core.Models;
namespace MiniHawraman.Core.Services.Interfaces
{
    public interface IUserService
    {
        User GetUser(int id);
        User GetUser(string username);
        IEnumerable<User> GetUsers();
        void AddUser(User user);
        void EditUser(User user);
        void DeleteUser(int id);
        bool ValidateUser(string username, string password);
        void SetValidationCode(string username, string code);
        string GetValidationCode(string username);
        bool ChangePassword(string username, string oldPassword, string newPassword);
    }
}
