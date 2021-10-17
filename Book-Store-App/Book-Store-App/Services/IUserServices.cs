namespace Book_Store_App.Services
{
    public interface IUserServices
    {
        string GetCurrentUserId();
        bool IsAuth();
    }
}