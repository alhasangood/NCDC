using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using Management.ViewModels;
using Managment.Controllers;
using Management.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Management.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Common.Captcha;
using Portal.Attributes;

namespace Management.Controllers
{
    [Route("api/[controller]")]
    public class SecurityController : RootController
    {
        private readonly SecuritySettings securitySettings;
        private readonly ILogger<SecurityController> logger;

        public SecurityController(NCDCContext context, IOptions<SecuritySettings> Configuration) : base(context)
        {
            securitySettings = Configuration.Value;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        [ExceptionCode("EX01001")]
        public async Task<IActionResult> Login([FromBody] LoginVM loginVM)
        {
           
                string Captcha = HttpContext.Session.GetString("Captcha");

                if (!loginVM.Captcha.Equals(Captcha, StringComparison.CurrentCultureIgnoreCase))
                {
                    return BadRequest(new { statusCode = "RE0601", message = "رمز التحقق غير صحيح" });
                }

                var user = await (from u in db.Users
                                  where u.LoginName == loginVM.LoginName && u.Status != Status.Deleted
                                  select new
                                  {
                                      u,
                                   //   centerName = u.UserType == UserTypes.HealthUser ? u.HealthCenter.HealthCenterName : u.UserType == UserTypes.RegionUser ? u.RegionCenter.RegionsCenterName : "المركز الوطني لمكافحة السرطان",
                                      Permissions = u.UserPermissionsUser.Select(p => p.Permission.Code).ToList()
                                  }).SingleOrDefaultAsync();

                if (user is null)
                {
                    return BadRequest(new { statusCode = "RE0901", message = "اسم المستخدم أو كلمة المرور غير صحيح" });
                }

                if (user.u.Status == Status.Locked)
                {
                    return BadRequest(new { statusCode = "RE0903", message = "حسابك مجمد ولا يمكنك تسجيل الدخول" });
                }

                if (user.u.Status == Status.Inactive)
                {
                    //if (user.u.LockedOn > DateTime.Now.AddMinutes(-1 * securitySettings.LockPeriod))
                    //{
                    //    return BadRequest(new { statusCode = "RE0904", message = "حسابك موقوف مؤقتاً، الرجاء الانتظار لخمس دقائق والمحاولة مرة اخرى." });
                    //}

                    user.u.Status = Status.Active;
                    await db.SaveChangesAsync();
                }

                string hashPassword = Security.ComputeHash(loginVM.Password, Security.Base64Decode("QXJAcUBUZWNoMjAxOCE="));

                if (string.Compare(hashPassword, user.u.Password, false) != 0)
                {
                    //user.u.LoginAttempts++;
                    //if (user.u.LoginAttempts >= securitySettings.MaxLoginAttempts)
                    //{
                    //    user.u.Status = Status.Inactive;
                    //    user.u.LockedOn = DateTime.Now;
                    //}
                    await db.SaveChangesAsync();
                    return BadRequest(new { statusCode = "RE0905", message = "اسم المستخدم أو كلمة المرور غير صحيح" });
                }

                user.u.LastLoginOn = DateTime.Now;
                //user.u.LoginAttempts = 0;

                await db.SaveChangesAsync();

                const string Issuer = "http://www.coretec.ly";
                var claims = new List<Claim>
                {
                    new Claim("UserId", user.u.UserId.ToString(), ClaimValueTypes.Integer64, Issuer),
                    new Claim("UserName", user.u.FullName, ClaimValueTypes.Integer64, Issuer),
                    new Claim("UserTypeId", user.u.UserTypeId.ToString(), ClaimValueTypes.Integer64, Issuer),
                };

                var userIdentity = new ClaimsIdentity("CORETEC@2020SECURITY");
                userIdentity.AddClaims(claims);
                var userPrincipal = new ClaimsPrincipal(userIdentity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal,
                    new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(60),
                        IsPersistent = true,
                        AllowRefresh = true
                    });

                var authenticatedUser = new
                {
                    user.u.UserId,
                    user.u.FullName,
                    user.Permissions
                };

                return Ok(new { statusCode = 1, authenticatedUser });

        }

        [AllowAnonymous]
        [HttpGet("LoginStatus")]
        [ExceptionCode("EX01001")]
        public async Task<IActionResult> LoginStatus()
        {
           
                var userId = UserId();

                if (userId <= 0)
                {
                    return Unauthorized(new { statusCode = "RE0906", result = "الرجاء إعادة تسجيل الدخول" });
                }

                var user = (from a in db.Users
                            where a.UserId == userId
                            select new
                            {
                                a.UserId,
                                a.FullName,
                                a.UserTypeId,
                           //     centerName = a.UserTypeId == UserTypes.HealthUser ? a.HealthCenter.HealthCenterName : a.UserType == UserTypes.RegionUser ? a.RegionCenter.RegionsCenterName : "المركز الوطني لمكافحة السرطان",

                                Permissions = a.UserPermissionsUser.Select(p => p.Permission.Code).ToList()
                            }).SingleOrDefault();

                const string Issuer = "http://www.coretec.ly";
                var claims = new List<Claim>
                {
                     new Claim("UserId", user.UserId.ToString(), ClaimValueTypes.Integer64, Issuer),
                //    new Claim("UserName", user.FullName, ClaimValueTypes.Integer64, Issuer),
                  //  new Claim("UserTypeId", user.UserType.ToString(), ClaimValueTypes.Integer64, Issuer),
                };

                var userIdentity = new ClaimsIdentity("CORETEC@2020SECURITY");
                userIdentity.AddClaims(claims);
                var userPrincipal = new ClaimsPrincipal(userIdentity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal,
                    new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(60),
                        IsPersistent = true,
                        AllowRefresh = true
                    });

                var authenticatedUser = new
                {
                    user.UserId,
                    user.FullName,
                   // user.centerName,
                    user.Permissions,
                };

                return Ok(new { statusCode = 1, authenticatedUser });
          
        }


        [HttpPost("Logout")]
        [ExceptionCode("EX01001")]
        public async Task<IActionResult> Logout()
        {
           
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                return Ok(new { statusCode = 1, message = "تم تسجيل الخروج بنجاح" });
           
        }

        [AllowAnonymous]
        [HttpGet("Captcha")]
        [ExceptionCode("EX01001")]
        public ActionResult Captcha()
        {
            FileContentResult result;

            CaptchaImage myCaptcha = new CaptchaImage();

            HttpContext.Session.SetString("Captcha", myCaptcha.Text);

            myCaptcha.Width = 220;
            var b = myCaptcha.RenderImage();

            using (var memStream = new System.IO.MemoryStream())
            {
                b.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                result = File(memStream.GetBuffer(), "image/jpeg");
                memStream.Close();
            }

            return result;
        }

    }
}
