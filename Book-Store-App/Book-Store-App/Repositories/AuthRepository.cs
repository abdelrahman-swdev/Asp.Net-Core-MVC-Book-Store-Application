using Book_Store_App.Data;
using Book_Store_App.Interfaces;
using Book_Store_App.Models;
using Book_Store_App.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_Store_App.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserServices _userServices;
        private readonly IEmailServices _emailServices;
        private readonly IConfiguration _configuration;

        public AuthRepository(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserServices userServices,
            IEmailServices emailServices,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userServices = userServices;
            _emailServices = emailServices;
            _configuration = configuration;
        }
        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
        {
            IdentityResult result =  await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                if (!string.IsNullOrEmpty(token))
                {
                    // send email for verification
                    await SendEmailConfirmationToken(user, token);
                }
            }
            return result;
        }

        public async Task<SignInResult> LoginAsync(LoginModel model)
        {
            SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            return result;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel model)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(_userServices.GetCurrentUserId());
            if(user != null)
            {
                return await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            }
            return null;
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string uid, string token)
        {
            return await _userManager.ConfirmEmailAsync(await _userManager.FindByIdAsync(uid), token);
        }

        public async Task SendForgotPasswordEmailAsync(ForgotPasswordModel model)
        {
            ApplicationUser user =  await _userManager.FindByEmailAsync(model.Email);
            if(user != null)
            {
                string token =  await _userManager.GeneratePasswordResetTokenAsync(user);
                var appDomain = _configuration.GetSection("Application:AppDomain").Value;
                var ResetPassword = _configuration.GetSection("Application:ResetPassword").Value;
                var mailModel = new MailRequestModel()
                {
                    ToEmails = new List<string>() { user.Email },
                    PlaceHolders = new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("[[link]]", string.Format(appDomain + ResetPassword, user.Id , token))
                    },
                    Subject = "Bookly, Reset Password"
                };

                await _emailServices.SendingResetPasswordToken(mailModel);

            }
        }

        public async Task<bool> ConfirmResetPassword(string uid, string token)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(uid);
            var purpose = UserManager<ApplicationUser>.ResetPasswordTokenPurpose;

            bool check = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider,purpose, token);
            return check;
        }

        public async Task<IdentityResult> ResetPassword(ResetPasswordModel model)
        {
            return await _userManager.ResetPasswordAsync( await _userManager.FindByIdAsync(model.UserId), model.Token, model.NewPassword);
        }

        private async Task SendEmailConfirmationToken(ApplicationUser user, string token)
        {
            var appDomain = _configuration.GetSection("Application:AppDomain").Value;
            var emailConfirmation = _configuration.GetSection("Application:EmailConfirmation").Value;
            var model = new MailRequestModel()
            {
                ToEmails = new List<string>() { user.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("[[username]]", user.FirstName + " " + user.LastName),
                    new KeyValuePair<string, string>("[[link]]", string.Format(appDomain + emailConfirmation, user.Id , token))
                },
                Subject = "Subject From Bookly Web App"
            };

            await _emailServices.SendingEmailConfirmationToken(model);
        }


    }
}
