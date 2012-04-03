
namespace MiniHawraman.Core.Services.Interfaces
{
    public interface IFormsAuthentication
    {
        void SignIn(string userName, bool createPersistentCookie);
        void SignOut();
    }
}
