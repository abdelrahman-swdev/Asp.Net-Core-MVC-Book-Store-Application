using Book_Store_App.Data;
using Book_Store_App.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_Store_App.Interfaces
{
    public interface IAuthRepository
    {
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        Task<SignInResult> LoginAsync(LoginModel model);

        Task LogoutAsync();

        Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel model);

        Task<IdentityResult> ConfirmEmailAsync(string uid, string token);

        Task SendForgotPasswordEmailAsync(ForgotPasswordModel model);

        Task<bool> ConfirmResetPassword(string uid, string token);

        Task<IdentityResult> ResetPassword(ResetPasswordModel model);
    }
}
