namespace MYChamp.Controller
{
    public interface ISessionHandlerService
    {
        bool SessionExists(string userId);
        void AddSessionInformation(string sessionId, string userId, string ip, DateTime loggedTime);
        void UpdateSessionInformation(string sessionId, string userName);
        bool check(string uniqueId);
        void UpdateForceLogout(string username, string uniqueId);
        bool checkForcefulLogout();
    }
}
