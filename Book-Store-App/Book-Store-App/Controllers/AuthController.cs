using AutoMapper;
using Book_Store_App.Data;
using Book_Store_App.Interfaces;
using Book_Store_App.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Book_Store_App.Controllers
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(IAuthRepository authRepository, IMapper mapper,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _authRepository = authRepository;
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        [HttpGet("signup")]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = _mapper.Map<ApplicationUser>(model);
                IdentityResult result = await _authRepository.CreateUserAsync(user, model.Password);

                if(result.Succeeded)
                {
                    return RedirectToAction("Login",new { verifiyMsg = "Account created successfully, check your email for verification link" });
                }
                else
                {
                    foreach(var er in result.Errors)
                    {
                        ModelState.AddModelError("", er.Description);
                    }
                    return View(model);
                }
            }
            return View(model);
        }


        [HttpGet("login")]
        public async Task<IActionResult> Login([FromQuery]string returnUrl, string verifiyMsg = "", bool isConfirmed = false)
        {
            ViewBag.verifiyMsg = verifiyMsg;
            LoginModel model = new LoginModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            return View(model);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model, [FromQuery]string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _authRepository.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && !returnUrl.Contains("login") && !returnUrl.Contains("signup")
                        && !returnUrl.Contains("forgot-password") && !returnUrl.Contains("reset-password"))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                if (result.IsNotAllowed)
                {
                    ModelState.AddModelError("", "Not Allowed To Login");
                    return View(model);
                }
                ModelState.AddModelError("", "Invalid Details");
                return View(model);
            }
            return View(model);
        }

        [HttpPost("external-login")]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallBack", "Auth", new { ReturnUrl = returnUrl});
            AuthenticationProperties properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider,properties);
        }

        [Route("external-login-callback")]
        public async Task<IActionResult> ExternalLoginCallBack([FromQuery]string returnUrl, string providerErrors = null)
        {
            // user authenticated by google
            returnUrl = returnUrl ?? "~/";

            LoginModel model = new LoginModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (providerErrors != null)
            {
                ModelState.AddModelError("", $"error from google: {providerErrors}");
                return View(nameof(Login),model);
            }

            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if(info == null)
            {
                ModelState.AddModelError("", "failed to load information from external login provider");
                return View(nameof(Login), model);
            }

            // user information returned from external login provider
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, true);
            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                if (!string.IsNullOrEmpty(email))
                {
                    ApplicationUser user = await _userManager.FindByEmailAsync(email);
                    if(user == null)
                    {
                        user = new ApplicationUser { 
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                            FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName),
                            LastName = info.Principal.FindFirstValue(ClaimTypes.Surname)
                        };

                        await _userManager.CreateAsync(user);
                    }

                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    if (!string.IsNullOrEmpty(returnUrl) && !returnUrl.Contains("login") && !returnUrl.Contains("signup")
                        && !returnUrl.Contains("forgot-password") && !returnUrl.Contains("reset-password")
                        && !returnUrl.Contains("external-login") && !returnUrl.Contains("external-login-callback"))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ViewBag.ErrorTitle = $"Email claim not recieved from: {info.ProviderDisplayName}";
                    return View("EmailClaimNotRecievedError");
                }

            }

        }

        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authRepository.LogoutAsync();
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet("change-password")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authRepository.ChangePasswordAsync(model);
                if(result == null)
                {
                    ModelState.AddModelError("", "Invalid Details");
                    return View(model);
                }
        
                if (result.Succeeded)
                {
                    ViewBag.changepasswordsucceeded = "Password changed successfully";
                    ModelState.Clear();
                    return View(model);
                }
                else
                {
                    foreach (IdentityError er in result.Errors)
                    {
                        ModelState.AddModelError("", er.Description);
                    }
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet("forgot-password")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            await _authRepository.SendForgotPasswordEmailAsync(model);
            return View("ForgotPasswordTokenSentSuccessfully");
        }

        [HttpGet("reset-password")]
        public async Task<IActionResult> ResetPassword(string uid, string token)
        {
            if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(token))
            {
                token = token.Replace(' ', '+');
                bool check = await _authRepository.ConfirmResetPassword(uid, token);
                if (check)
                {
                    var model = new ResetPasswordModel
                    {
                        Token = token,
                        UserId = uid
                    };
                    return View(model);
                }
            }
            return Content("some thing went wrong, try again");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                model.Token = model.Token.Replace(' ', '+');
                var result = await _authRepository.ResetPassword(model);
                if (result.Succeeded)
                {
                    ModelState.Clear();
                    return View("ResetPasswordSuccessfully");
                }
                else
                {
                    foreach(var er in result.Errors)
                    {
                        ModelState.AddModelError("", er.Description);
                    }
                }
            }
            return View(model);
        }


        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string uid, string token)
        {
            if(!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(token))
            {
                token = token.Replace(' ', '+');
                var result = await _authRepository.ConfirmEmailAsync(uid, token);
                if (result.Succeeded)
                {
                    ViewBag.isConfirmed = true;
                }
            }

            return View();
        }
    }
}
