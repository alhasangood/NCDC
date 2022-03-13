using Common;
using FluentEmail.Core;
using Management.Services;
using Management.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 
namespace Managment.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : RootController
    {
        private readonly ILogger<UsersController> logger;
        private readonly IFluentEmail email;
        private readonly IConfiguration configuration;
        private string path;

        public UsersController(NCDCContext context, IFluentEmail email, ILogger<UsersController> logger, IConfiguration configuration) : base(context)
        {
            this.logger = logger;
            this.email = email;
            this.configuration = configuration;
            path = $"{this.configuration["Settings:Domain"].ToString()}{"/img/nccp.svg"}";
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Pagination pagination, [FromQuery] UsersFilterVM filter)
        {
            try
            {
                if (!HasPermission("05000")) return BadRequest(new { statusCode = "AE0501", message = AppMessages.noPermission });
                var query = from c in db.Users.Where(p => p.Status != Status.Deleted)
                                    .WhereIf(filter.Search is not null, u => u.LoginName.Contains(filter.Search))
                            orderby c.CreatedOn descending
                            select new UsersListVM
                            {
                                CreatedBy = c.CreatedByNavigation.FullName,
                                CreatedOn = c.CreatedOn.ToString("yyyy/MM/dd"),
                                LoginName = c.LoginName,
                                Status = c.Status,
                                //UserId = c.UserId,/
                            //    JobDescription = c.JobDescription,
                                //ParentCenter = c.UserType == UserTypes.RegionUser ? c.RegionCenter.RegionsCenterName
                                //    : c.UserType == UserTypes.HealthUser ? c.HealthCenter.HealthCenterName
                                //    : "لا يوجد",
                                //UserType = c.UserType
                            };

                var totalItems = await query.CountAsync();
                var users = await query.Skip(pagination.PageSize * (pagination.Page - 1)).Take(pagination.PageSize).ToListAsync();
                var result = new { statusCode = 1, users, totalItems };

                return Ok(result);
            }
            catch (Exception ex)
            {
                var log = Logging.CreateLog("EX0501", HttpContext);

                logger.LogCritical(ex, "{@errorCode} {@method} {@path} {@queryString} {@userName}",
                                        log.ErrorCode, log.Method, log.Path, log.QueryString, log.UserName);
                return StatusCode(500, new { statusCode = log.ErrorCode, message = AppMessages.ServerError });
            }
        }

        [AllowAnonymous]
        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetDetails(int id)
        {
            try
            {
                if (!HasPermission("05003")) return BadRequest(new { statusCode = "AE0502", message = AppMessages.noPermission });

                //var user = await (from c in db.Users
                //                  where c.UserId == id
                //                  && c.Status != Status.Deleted
                //                  select new UserDetailsVM
                //                  {
                //                      CreatedBy = c.CreatedByNavigation.FullName,
                //                      CreatedOn = c.CreatedOn.ToString("yyyy/MM/dd"),
                //                      Email = c.Email,
                //                      FullName = c.FullName,
                //                      LoginName = c.LoginName,
                //                      PhoneNo = c.PhoneNo,
                //                      Status = c.Status,
                //                      UpdatedBy = c.UpdatedByNavigation.FullName,
                //                      UpdatedOn = c.UpdatedOn.Value.ToString("yyyy/MM/dd"),
                //                      JobDescription = c.JobDescription,
                //                      UserType = c.UserType,
                //                      ParentCenter = c.UserType == UserTypes.RegionUser ? c.RegionCenter.RegionsCenterName
                //                    : c.UserType == UserTypes.HealthUser ? c.HealthCenter.HealthCenterName
                //                    : "",
                //                      Features = (from f in db.Features
                //                                  where f.Status != Status.Deleted
                //                                  select new Features
                //                                  {
                //                                      FeatureId = f.FeatureId,
                //                                      FeatureName = f.FeatureName,
                //                                      Permissions = (from o in db.UserPermissions
                //                                                     where o.UserId == id
                //                                                     && o.Permission.FeatureId == f.FeatureId
                //                                                     select new Permissions
                //                                                     {
                //                                                         PermissionId = o.PermissionId,
                //                                                         PermissionName = o.Permission.PermissionName
                //                                                     }).ToList(),
                //                                  }).Where(x => x.Permissions.Count > 0).ToList(),
                //                  }).SingleOrDefaultAsync();
                var user = await (from c in db.Users
                                  where c.UserId == id
                                  && c.Status != Status.Deleted
                                  select new UserDetailsVM
                                  {
                                      CreatedBy = c.CreatedByNavigation.FullName,
                                      CreatedOn = c.CreatedOn.ToString("yyyy/MM/dd"),
                                      Email = c.Email,
                                      FullName = c.FullName,
                                      LoginName = c.LoginName,
                                      PhoneNo = c.PhoneNo,
                                  }).SingleOrDefaultAsync();
                if (user is null) return NotFound(new { statusCode = "RE0501", message = "لم يتم العثور على بيانات المستخدم" });

                var result = new { statusCode = 1, user };

                return Ok(result);
            }
            catch (Exception ex)
            {
                var log = Logging.CreateLog("EX0502", HttpContext);
                logger.LogCritical(ex, "{@errorCode} {@method} {@path} {@queryString} {@userName}",
                                        log.ErrorCode, log.Method, log.Path, log.QueryString, log.UserName);
                return StatusCode(500, new { statusCode = log.ErrorCode, message = AppMessages.ServerError });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] UserVM dataVM)
        {
            try
            {
                if (!HasPermission("05001")) return BadRequest(new { statusCode = "AE0503", message = AppMessages.noPermission });
                var dublicateName = await (from c in db.Users
                                           where c.LoginName == dataVM.LoginName
                                           && c.Status != Status.Deleted
                                           select c).AnyAsync();
                if (dublicateName) return BadRequest(new { StatusCode = "RE0502", message = "لا يمكن تخزين اسم مستخدم مسجل مسبقا" });


                var newuser = mapper.Map<User>(dataVM);

                newuser.CreatedBy = UserId();
                Guid randomGuid = Guid.NewGuid();
                int x = randomGuid.ToString().IndexOf("-");
                string password = randomGuid.ToString().Substring(0, x);
                newuser.Password = Security.ComputeHash(password, Security.Base64Decode("QXJAcUBUZWNoMjAxOCE="));

                db.Users.Add(newuser);

                dataVM.Permissions.ForEach(x => {
                    var UserPermission = new UserPermission
                    {
                        CreatedBy = UserId(),
                        UserId = newuser.UserId,
                        CreatedOn = DateTime.Now,
                        PermissionId = x
                    };

                    newuser.UserPermissionUsers.Add(UserPermission);
                });


                for (var attempts = 0; attempts < 3; attempts++)
                {

                    var sendingEmail = await email.To($"{newuser.Email}").Subject("اعادة تعيين كلمة المرور الخاصة بحسابك")
                          .Body(BodyEmail("كلمة المرور الخاصة بحسابك", newuser.LoginName, password), true).SendAsync();

                    if (sendingEmail.Successful) break;
                    else if (!sendingEmail.Successful && attempts < 2)
                    {
                        var ex = string.Join(",", sendingEmail.ErrorMessages);
                        var log = Logging.CreateLog("EX0502", HttpContext);
                        logger.LogCritical(ex, "{@errorCode} {@method} {@path} {@queryString} {@userName}",
                                                log.ErrorCode, log.Method, log.Path, log.QueryString, log.UserName);
                    }
                    else if (!sendingEmail.Successful && attempts == 2)
                    {
                        var ex = string.Join(",", sendingEmail.ErrorMessages);
                        var log = Logging.CreateLog("EX0502", HttpContext);

                        logger.LogCritical(ex, "{@errorCode} {@method} {@path} {@queryString} {@userName}",
                                                log.ErrorCode, log.Method, log.Path, log.QueryString, log.UserName);
                        return BadRequest(new { statusCode = "RE0502", message = "لم تنجح عملية ارسال كلمة المرور عبر البريد الكتروني" });
                    }
                }
                await db.SaveChangesAsync();


                var user = await (from c in db.Users
                                  where c.UserId == newuser.UserId
                                  select new UsersListVM
                                  {
                                      CreatedBy = c.CreatedByNavigation.FullName,
                                      CreatedOn = c.CreatedOn.ToString("yyyy/MM/dd"),
                                      LoginName = c.LoginName,
                                      Status = c.Status,
                                      UserId = c.UserId,
                                      JobDescription = c.JobDescription,
                                      ParentCenter = c.UserType == UserTypes.RegionUser ? c.RegionCenter.RegionsCenterName
                                    : c.UserType == UserTypes.HealthUser ? c.HealthCenter.HealthCenterName
                                    : "لا يوجد",
                                      UserType = c.UserType
                                  }).SingleOrDefaultAsync();
                var result = new { statusCode = 1, user, message = "تم إضافة المستخدم بنجاح" };
                return Ok(result);
            }
            catch (Exception ex)
            {
                var log = Logging.CreateLog("EX0503", HttpContext);

                logger.LogCritical(ex, "{@errorCode} {@method} {@path} {@queryString} {@userName}",
                                        log.ErrorCode, log.Method, log.Path, log.QueryString, log.UserName);
                return StatusCode(500, new { statusCode = log.ErrorCode, message = AppMessages.ServerError });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                if (!HasPermission("05002")) return BadRequest(new { statusCode = "AE0504", message = AppMessages.noPermission });

                var user = await (from c in db.Users
                                  where c.UserId == id
                                  && c.Status != Status.Deleted
                                  select new UserVM
                                  {
                                      LoginName = c.LoginName,
                                      FullName = c.FullName,
                                      PhoneNo = c.PhoneNo,
                                      Email = c.Email,
                                      JobDescription = c.JobDescription,
                                      HealthCenterId = c.HealthCenterId,
                                      RegionCenterId = c.RegionCenterId,
                                      UserType = c.UserType,
                                      Permissions = c.UserPermissionUsers.Select(x => x.PermissionId).ToList(),
                                  }).SingleOrDefaultAsync();

                if (user is null) return NotFound(new { statusCode = "RE0503", message = "لم يتم العثور على المستخدم" });

                var result = new { statusCode = 1, user };

                return Ok(result);
            }
            catch (Exception ex)
            {
                var log = Logging.CreateLog("EX0504", HttpContext);

                logger.LogCritical(ex, "{@errorCode} {@method} {@path} {@queryString} {@userName}",
                                        log.ErrorCode, log.Method, log.Path, log.QueryString, log.UserName);
                return StatusCode(500, new { statusCode = log.ErrorCode, message = AppMessages.ServerError });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] UserVM dataVM)
        {
            try
            {
                if (!HasPermission("05002")) return BadRequest(new { statusCode = "AE0505", message = AppMessages.noPermission });
                var dublicateName = await (from c in db.Users
                                           where c.LoginName == dataVM.LoginName
                                           && c.Status != Status.Deleted
                                           && c.UserId != id
                                           select c).AnyAsync();

                if (dublicateName) return BadRequest(new { StatusCode = "RE0504", message = "لا يمكن تخزين اسم مستخدم مسجل مسبقا" });

                var edituser = await (from c in db.Users
                                       .Include(x => x.UserPermissionUsers)
                                      where c.UserId == id
                                         && c.Status != Status.Deleted
                                      select c).SingleOrDefaultAsync();

                if (edituser is null) return BadRequest(new { StatusCode = "RE0505", message = "لا توجد بيانات" });
                mapper.Map(dataVM, edituser);
                edituser.HealthCenterId = dataVM.UserType is UserTypes.RegionUser or UserTypes.Manager ? null : dataVM.HealthCenterId;
                edituser.RegionCenterId = dataVM.UserType is UserTypes.HealthUser or UserTypes.Manager ? null : dataVM.RegionCenterId;
                edituser.UpdatedBy = UserId();

                foreach (var permission in edituser.UserPermissionUsers)
                {
                    db.UserPermissions.Remove(permission);
                }



                dataVM.Permissions.ForEach(x => {

                    var UserPermission = new UserPermission
                    {
                        CreatedBy = UserId(),
                        UserId = id,
                        CreatedOn = DateTime.Now,
                        PermissionId = x
                    };
                    edituser.UserPermissionUsers.Add(UserPermission);
                });


                await db.SaveChangesAsync();

                var user = await (from c in db.Users
                                  where c.UserId == edituser.UserId
                                  select new UsersListVM
                                  {
                                      CreatedBy = c.CreatedByNavigation.FullName,
                                      CreatedOn = c.CreatedOn.ToString("yyyy/MM/dd"),
                                      LoginName = c.LoginName,
                                      Status = c.Status,
                                      UserId = c.UserId,
                                      JobDescription = c.JobDescription,
                                      ParentCenter = c.UserType == UserTypes.RegionUser ? c.RegionCenter.RegionsCenterName
                                    : c.UserType == UserTypes.HealthUser ? c.HealthCenter.HealthCenterName
                                    : "لا يوجد",
                                      UserType = c.UserType
                                  }).SingleOrDefaultAsync();
                var result = new { statusCode = 1, user, message = "تم تعديل المستخدم بنجاح" };
                return Ok(result);
            }
            catch (Exception ex)
            {
                var log = Logging.CreateLog("EX0505", HttpContext);

                logger.LogCritical(ex, "{@errorCode} {@method} {@path} {@queryString} {@userName}",
                                        log.ErrorCode, log.Method, log.Path, log.QueryString, log.UserName);
                return StatusCode(500, new { statusCode = log.ErrorCode, message = AppMessages.ServerError });
            }
        }

        [HttpPut("{id}/lock")]
        public async Task<IActionResult> Lock(int id)
        {
            try
            {
                if (!HasPermission("05004")) return BadRequest(new { statusCode = "AE0506", message = AppMessages.noPermission });

                var user = await (from c in db.Users
                                  where c.UserId == id
                                  && c.Status != Status.Deleted
                                  select c).SingleOrDefaultAsync();

                if (user is null) return NotFound(new { statusCode = "RE0506", message = "لم يتم العثور على المستخدم" });

                if (user.Status == Status.Locked) return NotFound(new { statusCode = "RE0507", message = "هذا المستخدم مجمد مسبقا" });
                if (user.Status != Status.Active) return NotFound(new { statusCode = "RE0508", message = "هذا المستخدم غير مفعل" });

                user.Status = Status.Locked;
                user.UpdatedBy = UserId();
                user.UpdatedOn = DateTime.Now;

                await db.SaveChangesAsync();

                var result = new { statusCode = 1, message = "تم تجميد المستخدم بنجاح" };

                return Ok(result);
            }
            catch (Exception ex)
            {
                var log = Logging.CreateLog("EX0506", HttpContext);

                logger.LogCritical(ex, "{@errorCode} {@method} {@path} {@queryString} {@userName}",
                                        log.ErrorCode, log.Method, log.Path, log.QueryString, log.UserName);
                return StatusCode(500, new { statusCode = log.ErrorCode, message = AppMessages.ServerError });
            }
        }

        [HttpPut("{id}/unlock")]
        public async Task<IActionResult> Unlock(int id)
        {
            try
            {
                if (!HasPermission("05004")) return BadRequest(new { statusCode = "AE0507", message = AppMessages.noPermission });

                var user = await (from c in db.Users
                                  where c.UserId == id
                                  && c.Status != Status.Deleted
                                  select c).SingleOrDefaultAsync();

                if (user is null) return NotFound(new { statusCode = "RE0509", message = "لم يتم العثور على المستخدم" });

                if (user.Status == Status.Active) return NotFound(new { statusCode = "RE0510", message = "هذا المستخدم مفعل مسبقا" });

                user.Status = Status.Active;
                user.UpdatedBy = UserId();
                user.UpdatedOn = DateTime.Now;

                await db.SaveChangesAsync();

                var result = new { statusCode = 1, message = "تم فك تجميد المستخدم بنجاح" };

                return Ok(result);
            }
            catch (Exception ex)
            {
                var log = Logging.CreateLog("EX0507", HttpContext);

                logger.LogCritical(ex, "{@errorCode} {@method} {@path} {@queryString} {@userName}",
                                        log.ErrorCode, log.Method, log.Path, log.QueryString, log.UserName);
                return StatusCode(500, new { statusCode = log.ErrorCode, message = AppMessages.ServerError });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (!HasPermission("05005")) return BadRequest(new { statusCode = "AE0508", message = AppMessages.noPermission });

                var user = await (from c in db.Users
                                  where c.UserId == id
                                  select c).SingleOrDefaultAsync();

                if (user is null) return NotFound(new { statusCode = "RE0511", message = "لم يتم العثور على المستخدم" });


                user.Status = Status.Deleted;
                user.UpdatedBy = UserId();
                user.UpdatedOn = DateTime.Now;

                await db.SaveChangesAsync();

                var result = new { statusCode = 1, message = "تم حذف المستخدم بنجاح" };

                return Ok(result);
            }
            catch (Exception ex)
            {
                var log = Logging.CreateLog("EX0508", HttpContext);

                logger.LogCritical(ex, "{@errorCode} {@method} {@path} {@queryString} {@userName}",
                                        log.ErrorCode, log.Method, log.Path, log.QueryString, log.UserName);
                return StatusCode(500, new { statusCode = log.ErrorCode, message = AppMessages.ServerError });
            }
        }

        [HttpGet("{id}/ResetPassword")]
        public async Task<IActionResult> ResetPassword(int id)
        {
            try
            {
                if (!HasPermission("05007")) return BadRequest(new { statusCode = "AE0509", message = AppMessages.noPermission });

                var user = await (from c in db.Users
                                  where c.UserId == id
                                  select c).SingleOrDefaultAsync();

                if (user is null) return NotFound(new { statusCode = "RE0512", message = "لم يتم العثور على المستخدم" });


                Guid randomGuid = Guid.NewGuid();
                int x = randomGuid.ToString().IndexOf("-");
                string password = randomGuid.ToString().Substring(0, x);
                user.Password = Security.ComputeHash(password, Security.Base64Decode("QXJAcUBUZWNoMjAxOCE="));
                user.UpdatedBy = UserId();
                user.UpdatedOn = DateTime.Now;

                for (var attempts = 0; attempts < 3; attempts++)
                {
                    var sendingEmail = await email.To($"{user.Email}").Subject("اعادة تعيين كلمة المرور الخاصة بحسابك")
                            .Body(BodyEmail("اعادة تعيين كلمة المرور", user.LoginName, password), true).SendAsync();


                    if (sendingEmail.Successful) break;
                    else if (!sendingEmail.Successful && attempts < 2)
                    {
                        var ex = string.Join(",", sendingEmail.ErrorMessages);
                        var log = Logging.CreateLog("EX0509", HttpContext);
                        logger.LogCritical(ex, "{@errorCode} {@method} {@path} {@queryString} {@userName}",
                                                log.ErrorCode, log.Method, log.Path, log.QueryString, log.UserName);
                    }
                    else if (!sendingEmail.Successful && attempts == 2)
                    {
                        var ex = string.Join(",", sendingEmail.ErrorMessages);
                        var log = Logging.CreateLog("EX0509", HttpContext);

                        logger.LogCritical(ex, "{@errorCode} {@method} {@path} {@queryString} {@userName}",
                                                log.ErrorCode, log.Method, log.Path, log.QueryString, log.UserName);
                        return BadRequest(new { statusCode = "RE05012", message = "لم تنجح عملية ارسال كلمة المرور عبر البريد الكتروني" });
                    }
                }

                await db.SaveChangesAsync();

                var result = new { statusCode = 1, message = "تم ارسال كلمة المرور الجديدة الى بريد المستخدم بنجاح" };

                return Ok(result);
            }
            catch (Exception ex)
            {
                var log = Logging.CreateLog("EX0509", HttpContext);

                logger.LogCritical(ex, "{@errorCode} {@method} {@path} {@queryString} {@userName}",
                                        log.ErrorCode, log.Method, log.Path, log.QueryString, log.UserName);
                return StatusCode(500, new { statusCode = log.ErrorCode, message = AppMessages.ServerError });
            }
        }

        [HttpGet("getFeatures")]
        public async Task<IActionResult> permissions()
        {
            try
            {
                if (!HasPermission("05001") || !HasPermission("05005") || !HasPermission("05003"))
                    return BadRequest(new { statusCode = "AE0510", message = AppMessages.noPermission });

                var usersPermissions = from u in db.UserPermissions
                                       where u.UserId == UserId()
                                       select u;
                var features = await (from f in db.Features
                                      where f.Status != Status.Deleted
                                      select new
                                      {
                                          f.FeatureId,
                                          f.FeatureName,
                                          checkAll = false,
                                          permissions = (from p in usersPermissions
                                                         where p.Permission.FeatureId == f.FeatureId
                                                         select new
                                                         {
                                                             p.PermissionId,
                                                             p.Permission.PermissionName,
                                                             p.Permission.Code
                                                         }).ToList()
                                      }).ToListAsync();

                return Ok(new { statusCode = 1, features });
            }
            catch (Exception ex)
            {
                var log = Logging.CreateLog("EX0510", HttpContext);

                logger.LogCritical(ex, "{@errorCode} {@method} {@path} {@queryString} {@userName}",
                                        log.ErrorCode, log.Method, log.Path, log.QueryString, log.UserName);
                return StatusCode(500, new { statusCode = log.ErrorCode, message = AppMessages.ServerError });
            }
        }
        private string BodyEmail(string title, string userName, string password)
        {
            String body = @$"
                <html>
                    <body style='background-color: #f4f4f5;'>
                      <table cellpadding='0' cellspacing='0' style='width:100%; height:100%;background-color:#f4f4f5;text-align:center;'>
                        <tbody>
                          <tr>
                            <td style='text-align: center;'>
                              <table align='center' cellpadding='0' cellspacing='0' id='body'
                                style='background-color:#fff;width:100%;max-width:680px; height:100%;'>
                                <tbody>
                                  <tr>
                                    <td>
                                      <table align='center' cellpadding='0' cellspacing='0'
                                        class='padding-left:0 !important;padding-right:0 !important;'
                                        style='text-align:left;padding-bottom:88px;width: 100%; padding-left: 120px; padding-right:120px;'>
                                        <tbody>
                                          <tr>
                                            <td style='padding-top:15px;'>
                                              <img src='{path}' style='width:200px;padding-left:72px'>
                                            </td>
                                          </tr>
                                          <tr>
                                            <td colspan='2'
                                              style='padding-top:30px;padding-left:72px;color: #000000; font-size:30px; font-smoothing: always; font-style: normal; font-weight: 600;'>
                                            {title}
                                            </td>
                                          </tr>
                                          <tr>
                                            <td style='padding-top: 48px; padding-bottom: 48px;'>
                                              <table cellpadding='0' cellspacing='0' style='width:100%'>
                                                <tbody>
                                                  <tr>
                                                    <td
                                                      style='width:100%; height: 1px; max-height: 1px; background-color: #d9dbe0; opacity: 0.81'>
                                                      </td>
                                                  </tr>
                                                  <tr>
                                                    <td>
                                                      <p align='right'> {userName} <b>: اسم المستخدم </b></p>
                                                    </td>
                                                  </tr>
                                                  <tr>
                                                    <td>
                                                      <p align='right'>{password} <b>: كلمة المرور </b></p>
                                                    </td>
                                                  </tr>
                                                </tbody>
                                                </table>
                                            </td>
                                          </tr>

                                        </tbody>
                                        </tabel>
                                    </td>
                                  </tr>
                                </tbody>
                                </tabel>
                            </td>
                          </tr>
                        </tbody>
                        </tabel>
                    </body>
            </html>";
            return body;
        }

    }



}
