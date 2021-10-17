using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Book_Store_App.Services
{
    public class UserServices : IUserServices
    {
        private readonly IHttpContextAccessor _http;

        public UserServices(IHttpContextAccessor http)
        {
            _http = http;
        }
        public string GetCurrentUserId()
        {
            return _http.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        public bool IsAuth()
        {
            return _http.HttpContext.User.Identity.IsAuthenticated;
        }
    }
}
