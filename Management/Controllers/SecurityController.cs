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

namespace Management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecurityController : ControllerBase
    {
        private NCDCContext db;
        public readonly string errorMsg = "The server could not process the request, please try again later or contact technical support ";


        public SecurityController(NCDCContext context)
        {
            db = context;
        }
        private int GetCurrentUserId()
        {
            try
            {
                var claims = HttpContext.User.Claims.ToList();
                int userId = Convert.ToInt32(claims.Where(p => p.Type == "UserId").Select(p => p.Value).SingleOrDefault());
                return userId;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        [HttpGet("CheckLogin")]
        [AllowAnonymous]
        public IActionResult CheckLogin()
        {
            try
            {
                var claims = HttpContext.User.Claims.ToList();
                int userId = Convert.ToInt32(claims.Where(p => p.Type == "UserId").Select(p => p.Value).SingleOrDefault());

                if (claims.Count <= 0 && userId <= 0)
                {
                    return Ok(new { StatusCode = 0, result = false });
                }
                return Ok(new { StatusCode = 0, result = true });
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString());
                return StatusCode(500, new { StatusCode = 99999, result = errorMsg });
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginForm loginForm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(loginForm.Email))
                {
                    return BadRequest(new { statusCode = 999002, result = "الرجاء التأكد من إدخال اسم الدخول" });
                }

                if (string.IsNullOrWhiteSpace(loginForm.Password))
                {
                    return BadRequest(new { statusCode = 999003, result = "الرجاء التأكد من إدخال كلمة المرور" });
                }

                var user = (from u in db.Users
                            where u.LoginName == loginForm.Email && u.Status != Status.Deleted
                            select u).SingleOrDefault();


                if (user == null)
                {
                    return NotFound(new { StatusCode = 999991, result = "لايمكنك الوصول الى هذا النظام" });
                }

                if (user.Status == Status.Locked)
                {
                    return BadRequest(new { StatusCode = 999992, result = "هذا المستخدم مجمد حاليا الرجاء مراجعة مشرف النظام" });
                }

                //if (user.Status == Status.TemproryLocked)
                //{
                //    if (user. > DateTime.Now.AddMinutes(-5))
                //    {
                //        return BadRequest(new { statusCode = 999008, result = "هذا المستخدم مجمد حاليا الرجاء مراجعة مشرف النظام" });
                //    }

                //    user.Status = Status.Active;
                //    user.LoginTryAttempts = 0;
                //}

                if (!Security.VerifyHash(loginForm.Password, user.Password, HashAlgorithms.SHA512))
                {
                    user.LoginTryAttempts++;
                    if (user.LoginTryAttempts >= 5 && user.Status == Status.Active)
                    {
                        user.Status = Status.TemproryLocked;
                        user.LastLoginOn = DateTime.Now;
                    }
                    db.SaveChanges();
                    return BadRequest(new { statusCode = 999009, result = "لقد قمت بإدخال اسم الدخول أو كلمة مرور غير صحيحة" });
                }

                user.LoginTryAttempts = 0;
                user.LastLoginOn = DateTime.Now;

                db.SaveChanges();

                const string Issuer = "http://www.GrowthTech.ly";
                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, user.FullName, ClaimValueTypes.String, Issuer));
                claims.Add(new Claim("UserId", user.UserId.ToString(), ClaimValueTypes.Integer64, Issuer));
                var userIdentity = new ClaimsIdentity("GrowthTech@2020SECURITY");
                userIdentity.AddClaims(claims);
                var userPrincipal = new ClaimsPrincipal(userIdentity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal,
                    new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(60),
                        IsPersistent = true,
                        AllowRefresh = true,
                    });

                var UserPermissions = (from p in db.UserPermissions
                                       where p.UserId == user.UserId
                                       select p.Permission.Code).ToList();

                var authenticatedUser = new
                {
                    userId = user.UserId,
                    userName = user.LoginName,
                    fullName = user.FullName,
                    userPermissions = UserPermissions

                };


                return Ok(new { statusCode = 0, result = authenticatedUser });
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "API ==> security Controller - login  /" +
                                                $"UserId ==> none /" +
                                                "Error Code ==> [999999] /" +
                                                $"Paramters ==>none");
                return StatusCode(500, new { statusCode = 999999, result = errorMsg });
            }
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Ok(new { statusCode = 0, result = "تم تسجيل الخروج بنجاح" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { statusCode = 999999, result = errorMsg });
            }

        }

    }
}
