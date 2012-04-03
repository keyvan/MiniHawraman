using MiniHawraman.Core.Services.Interfaces;

namespace MiniHawraman.Core.Services.Implementations
{
    public class MembershipService : IMembershipService
    {
        private UserService _userService;

        public MembershipService()
        {
        }

        public MembershipService(UserService userService)
        {
            this._userService = userService;
        }

        public bool ValidateUser(string userName, string password)
        {
            return this._userService.ValidateUser(userName, password);
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            return this._userService.ChangePassword(userName, oldPassword, newPassword);
        }
    }
}
