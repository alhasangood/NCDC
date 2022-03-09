using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Managment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : RootController
    {
        private readonly NCDCContext db;

        public UsersController(NCDCContext context) : base(context)
        {
            db = context;
        }


        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers(int pageNo, int pageSize, string searchByName)
        {
            try
            {
                //if (!GetUserPermission("09000"))
                //{
                //    return BadRequest(new { StatusCode = 919001, result = noPermission });
                //}

                if (pageNo <= 0)
                {
                    return BadRequest(new { StatusCode = 919002, result = "يرجي التأكد من البيانات" });
                }
                if (pageSize <= 0)
                {
                    return BadRequest(new { StatusCode = 919003, result = "يرجي التأكد من البيانات" });
                }

                var UsersQuery = from a in db.MaestroUsers
                                 where a.Status != Status.Deleted
                                 select a;

                if (!String.IsNullOrWhiteSpace(searchByName))
                {
                    UsersQuery = from a in UsersQuery
                                 where a.FullName.Contains(searchByName)
                                 select a;
                }

                var Users = await (from a in UsersQuery
                                   orderby a.MaestroUserId descending
                                   select new
                                   {
                                       a.MaestroUserId,
                                       a.LoginName,
                                       a.FullName,
                                       a.MobileNo,
                                       a.JobDesc,
                                       a.CreatedOn,
                                       a.Status
                                   }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();

                var totalItems = await UsersQuery.CountAsync();

                return Ok(new { StatusCode = 0, result = new { Users, totalItems } });
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "API ==>  Users Controller - GetUsers /" +
                            $"UserId ==> {UserId()} /" +
                            "Error Code ==> [919004] /" +
                            $"Paramters ==> pageNo = {pageNo} , pageSize= {pageSize} , searchByName = {searchByName}");
                return StatusCode(500, new { statusCode = 919004, result = errorMsg });
            }
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser([FromBody] UserData userInfo)
        {
            try
            {
                //if (!GetUserPermission("09001"))
                //{
                //    return BadRequest(new { StatusCode = 919005, result =noPermission });
                //}

                if (string.IsNullOrWhiteSpace(userInfo.FullName))
                {
                    return BadRequest(new { statusCode = 919006, result = "الرجاء التأكد من ادخال اسم المستخدم" });

                }
                if (userInfo.FullName.Length > 50)
                {
                    return BadRequest(new { statusCode = 919007, result = "الرجاء ادخال اسم مستخدم لا يتجاوز 50 حرف" });
                }
                if (string.IsNullOrWhiteSpace(userInfo.LoginName))
                {

                    return BadRequest(new { statusCode = 919008, result = "الرجاء التأكد من ادخال اسم الدخول" });
                }
                else if (userInfo.LoginName.Length > 30)
                {
                    return BadRequest(new { statusCode = 919009, result = "الرجاء ادخال اسم دخول لا يتجاوز 30 حرف" });
                }
                if (!String.IsNullOrWhiteSpace(userInfo.MobileNo))
                {
                    if (userInfo.MobileNo.Length < 9 || userInfo.MobileNo.Length > 10)
                    {
                        return BadRequest(new { statusCode = 919010, result = "الرجاء ادخال رقم الهاتف بصيغة 9 أو 10 أرقام" });
                    }
                }

                if (!String.IsNullOrWhiteSpace(userInfo.JobDesc))
                {
                    if (userInfo.JobDesc.Length > 50)
                    {
                        return BadRequest(new { statusCode = 919011, result = "الرجاء ادخال صفة لا يتجاوز 50 حرف" });
                    }
                }

                var checkLoginName = await (from a in db.Users
                                            where a.LoginName == userInfo.LoginName
                                            && a.Status != Status.Deleted
                                            select a).CountAsync();

                if (checkLoginName > 0)
                {
                    return BadRequest(new { statusCode = 919012, result = "اسم الدخول مستخدم مسبقا الرجاء ادخال اسم دخول اخر" });
                }
                //var randomGuid = Guid.NewGuid();
                //int x = randomGuid.ToString().IndexOf("-");
                //string password = randomGuid.ToString().Substring(0, x);

                MaestroUsers addUser = new MaestroUsers()
                {
                    FullName = userInfo.FullName,
                    JobDesc = userInfo.JobDesc,
                    LoginName = userInfo.LoginName,
                    MobileNo = userInfo.MobileNo,
                    Email = userInfo.Email,
                    Password = Security.ComputeHash(userInfo.Password, HashAlgorithms.SHA512, null),
                    LoginTryAttempts = 0,
                    CreatedBy = UserId(),
                    CreatedOn = DateTime.Now,
                    Status = Status.Active
                };
                await db.MaestroUsers.AddAsync(addUser);


                foreach (var permission in userInfo.SelectedPermissions)
                {
                    MaestroUserPermissions newPermission = new MaestroUserPermissions()
                    {
                        MaestroUserId = addUser.MaestroUserId,
                        PermissionId = permission,
                        CreatedBy = UserId(),
                        CreatedOn = DateTime.Now,
                    };
                    addUser.MaestroUserPermissions.Add(newPermission);
                }
                await db.SaveChangesAsync();

                var addedUser = await (from a in db.MaestroUsers
                                       where a.MaestroUserId == addUser.MaestroUserId
                                       select new
                                       {
                                           a.MaestroUserId,
                                           a.LoginName,
                                           a.FullName,
                                           a.MobileNo,
                                           a.JobDesc,
                                           a.CreatedOn,
                                           a.Status
                                       }).SingleOrDefaultAsync();

                return Ok(new { StatusCode = 0, result = new { message = "تم إضافة المستخدم بنجاح", addedUser } });
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "API ==>  Users Controller - AddUser /" +
                            $"UserId ==> {UserId()} /" +
                            "Error Code ==> [919013] /" +
                            $"Paramters ==> {ObjectLog.PrintPropreties(userInfo)}");

                return StatusCode(500, new { statusCode = 919013, result = errorMsg });
            }
        }

        [HttpGet("GetUserForView")]
        public async Task<IActionResult> GetUserForView(int userId)
        {
            try
            {
                if (userId <= 0)
                {
                    return BadRequest(new { statusCode = 919014, result = "الرجاء التأكد من اختيار مستخدم" });
                }
                //if (!GetUserPermission("09002"))
                //{
                //    return BadRequest(new { statusCode = 919015, result = "ليس لديك الصلاحيات اللازمة لتنفيذ الامر" });
                //}


                var userInfo = await (from a in db.MaestroUsers
                                      where a.MaestroUserId == userId
                                      && a.Status != Status.Deleted
                                      select new
                                      {
                                          a.MaestroUserId,
                                          a.LoginName,
                                          a.FullName,
                                          a.JobDesc,
                                          a.MobileNo,
                                          a.Email,
                                          selectedPermissions = (from f in db.Features
                                                                 join p in db.MaestroUserPermissions on f.FeatureId equals p.Permission.FeatureId
                                                                 where f.FeatureType == 1
                                                                 && p.MaestroUserId == userId
                                                                 select new
                                                                 {
                                                                     f.FeatureId,
                                                                     f.FeatureName,
                                                                     permissions = (from a in db.MaestroUserPermissions
                                                                                    where a.MaestroUserId == userId
                                                                                    && a.Permission.FeatureId == f.FeatureId
                                                                                    select a.Permission.PermissionName).ToList()
                                                                 }).Distinct().ToList(),

                                      }).SingleOrDefaultAsync();

                if (userInfo == null)
                {
                    return BadRequest(new { statusCode = 919016, result = "لم يتم العثور على هذا المستخدم" });
                }

                return Ok(new { statusCode = 0, result = new { userInfo } });
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "API ==> Users Controller - GetUserForView  /" +
                                $"UserId ==> {UserId()} /" +
                                "Error Code ==> [919017] /" +
                                $"Paramters ==> UserId = {userId}");
                return StatusCode(500, new { statusCode = 919017, result = errorMsg });
            }
        }

        [HttpGet("GetUserForEdit")]
        public async Task<IActionResult> GetUserForEdit(int userId)
        {
            try
            {


                if (userId < 0)
                {
                    return BadRequest(new { StatusCode = 919047, result = "يرجي التأكد من بيانات المستخدم." });
                }
                var userInfo = await (from a in db.MaestroUsers
                                      where a.MaestroUserId == userId
                                         && a.Status != Status.Deleted
                                      select new
                                      {
                                          a.MaestroUserId,
                                          a.LoginName,
                                          a.FullName,
                                          a.JobDesc,
                                          a.MobileNo,
                                          a.Email,
                                          selectedUserPermissions = (from p in db.MaestroUserPermissions
                                                                     where p.MaestroUserId == userId
                                                                     join u in db.MaestroUserPermissions.Where(z => z.MaestroUserId == UserId()) on p.PermissionId equals u.PermissionId
                                                                     select p.PermissionId).ToList()
                                      }).SingleOrDefaultAsync();

                return Ok(new { statusCode = 0, result = new { userInfo } });

            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "API ==> Users Controller - GetUserForEdit  /" +
                                       $"UserId ==> {UserId()} /" +
                                       "Error Code ==> [919048] /" +
                                       $"Paramters ==> UserId = {userId}");
                return StatusCode(500, new { statusCode = 919048, result = errorMsg });
            }
        }

        [HttpPut("EditrUser")]
        public async Task<IActionResult> EditUser([FromBody] UserData userInfo)
        {
            try
            {

                //if (!GetUserPermission("09003"))
                //{
                //    return BadRequest(new { StatusCode = 919018, result =noPermission });
                //}
                if (userInfo.MaestroUserId <= 0)
                {
                    return BadRequest(new { StatusCode = 919019, result = "يرجي اختيار المستخدم." });
                }

                if (string.IsNullOrWhiteSpace(userInfo.FullName))
                {
                    return BadRequest(new { statusCode = 919006, result = "الرجاء التأكد من ادخال اسم المستخدم" });

                }
                if (userInfo.FullName.Length > 50)
                {
                    return BadRequest(new { statusCode = 919007, result = "الرجاء ادخال اسم مستخدم لا يتجاوز 50 حرف" });
                }
                if (string.IsNullOrWhiteSpace(userInfo.LoginName))
                {

                    return BadRequest(new { statusCode = 919008, result = "الرجاء التأكد من ادخال اسم الدخول" });
                }
                else if (userInfo.LoginName.Length > 30)
                {
                    return BadRequest(new { statusCode = 919009, result = "الرجاء ادخال اسم دخول لا يتجاوز 30 حرف" });
                }
                if (!String.IsNullOrWhiteSpace(userInfo.MobileNo))
                {
                    if (userInfo.MobileNo.Length < 9 || userInfo.MobileNo.Length > 10)
                    {
                        return BadRequest(new { statusCode = 919010, result = "الرجاء ادخال رقم الهاتف بصيغة 9 أو 10 أرقام" });
                    }
                }

                if (!String.IsNullOrWhiteSpace(userInfo.JobDesc))
                {
                    if (userInfo.JobDesc.Length > 50)
                    {
                        return BadRequest(new { statusCode = 919011, result = "الرجاء ادخال صفة لا يتجاوز 50 حرف" });
                    }
                }

                var checkLoginName = await (from a in db.Users
                                            where a.LoginName == userInfo.LoginName
                                            && a.Status != Status.Deleted
                                            select a).CountAsync();

                if (checkLoginName > 0)
                {
                    return BadRequest(new { statusCode = 919012, result = "اسم الدخول مستخدم مسبقا الرجاء ادخال اسم دخول اخر" });
                }

                var userData = await (from a in db.MaestroUsers
                                      where a.MaestroUserId == userInfo.MaestroUserId
                                      select a).SingleOrDefaultAsync();

                if (userData == null)
                {
                    return BadRequest(new { statusCode = 919030, result = "لم يتم العثور على المستخدم" });
                }

                userData.FullName = userInfo.FullName;
                userData.JobDesc = userInfo.JobDesc;
                userData.LoginName = userInfo.LoginName;
                userData.MobileNo = userInfo.MobileNo;
                userData.ModifiedBy = UserId();
                userData.ModifiedOn = DateTime.Now;


                var oldPermissions = await (from a in db.MaestroUserPermissions
                                            where a.MaestroUserId == userInfo.MaestroUserId
                                            select a.PermissionId).ToListAsync();
                foreach (int permission in oldPermissions)
                {
                    if (userInfo.SelectedPermissions.Contains(permission))
                    {
                        userInfo.SelectedPermissions.RemoveAll(x => x == permission);
                    }
                    else
                    {
                        var removePermission = await (from p in db.MaestroUserPermissions
                                                      where p.PermissionId == permission
                                                      && p.MaestroUserId == userInfo.MaestroUserId
                                                      select p).SingleOrDefaultAsync();
                        db.MaestroUserPermissions.Remove(removePermission);
                    }
                }

                foreach (int permission in userInfo.SelectedPermissions)
                {
                    MaestroUserPermissions newPermission = new MaestroUserPermissions()
                    {
                        MaestroUserId = userData.MaestroUserId,
                        PermissionId = permission,
                        CreatedBy = UserId(),
                        CreatedOn = DateTime.Now
                    };
                    await db.MaestroUserPermissions.AddAsync(newPermission);
                }

                await db.SaveChangesAsync();

                return Ok(new { StatusCode = 0, result = new { result = "تم تعديل المستخدم بنجاح" } });
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "API ==>  Users Controller - EditUser /" +
                            $"UserId ==> {UserId()} /" +
                            "Error Code ==> [919027] /" +
                            $"Paramters ==> {ObjectLog.PrintPropreties(userInfo)}");
                return StatusCode(500, new { statusCode = 919027, result = errorMsg });
            }
        }

        [HttpPut("LockUser")]
        public async Task<IActionResult> LockUser(int userId)
        {
            try
            {
                if (userId <= 0)
                {
                    return BadRequest(new { statusCode = 919028, result = "الرجاء التأكد من البيانات " });
                }

                //if (!GetUserPermission("09004"))
                //{
                //    return BadRequest(new { statusCode = 919029, result = "ليس لديك الصلاحيات اللازمة لتنفيذ الامر" });
                //}

                var user = await (from a in db.MaestroUsers
                                  where a.MaestroUserId == userId
                                  && a.Status != Status.Deleted
                                  select a).SingleOrDefaultAsync();

                if (user == null)
                {
                    return BadRequest(new { statusCode = 919030, result = "لم يتم العثور على المستخدم" });
                }

                if (user.Status == Status.Locked)
                {
                    return BadRequest(new { statusCode = 919031, result = "تم تجميد المستخدم مسبقا" });
                }

                user.Status = Status.Locked;
                user.ModifiedBy = UserId();
                user.ModifiedOn = DateTime.Now;
                await db.SaveChangesAsync();

                return Ok(new { statusCode = 0, result = new { result = "تم تجميد المستخدم بنجاح" } });
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "API ==> Users Controller - LockUser  /" +
                                $"UserId ==> {UserId()} /" +
                                "Error Code ==> [919032] /" +
                                $"Paramters ==> UserId = {userId}");
                return StatusCode(500, new { statusCode = 919032, result = errorMsg });
            }
        }

        [HttpPut("UnlockUser")]
        public async Task<IActionResult> UnlockUser(int userId)
        {
            try
            {
                if (userId <= 0)
                {
                    return BadRequest(new { statusCode = 919033, result = "الرجاء التأكد من البيانات " });
                }

                //if (!GetUserPermission("09005"))
                //{
                //    return BadRequest(new { statusCode = 919034, result = "ليس لديك الصلاحيات اللازمة لتنفيذ الامر" });
                //}

                var user = await (from a in db.MaestroUsers
                                  where a.MaestroUserId == userId
                                  && a.Status != Status.Deleted
                                  select a).SingleOrDefaultAsync();

                if (user == null)
                {
                    return BadRequest(new { statusCode = 919035, result = "لم يتم العثور على المستخدم" });
                }

                if (user.Status == Status.Active)
                {
                    return BadRequest(new { statusCode = 919036, result = "هذا المستخدم مفعل مسبقا" });
                }

                user.Status = Status.Active;
                user.ModifiedBy = UserId();
                user.ModifiedOn = DateTime.Now;
                await db.SaveChangesAsync();

                return Ok(new { statusCode = 0, result = new { result = "تم فك تجميد المستخدم بنجاح" } });
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "API ==> rUsers Controller - UnLockUser  /" +
                                $"UserId ==> {UserId()} /" +
                                "Error Code ==> [919037] /" +
                                $"Paramters ==> UserId = {userId}");
                return StatusCode(500, new { statusCode = 919037, result = errorMsg });
            }
        }

        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                if (userId <= 0)
                {
                    return BadRequest(new { statusCode = 919038, result = "الرجاء التأكد من البيانات " });
                }

                //if (!GetUserPermission("09006"))
                //{
                //    return BadRequest(new { statusCode = 919039, result = "ليس لديك الصلاحيات اللازمة لتنفيذ الامر" });
                //}

                var user = await (from a in db.MaestroUsers
                                  where a.MaestroUserId == userId
                                  select a).SingleOrDefaultAsync();

                if (user == null)
                {
                    return BadRequest(new { statusCode = 919040, result = "لم يتم العثور على المستخدم" });
                }

                if (user.Status == Status.Deleted)
                {
                    return BadRequest(new { statusCode = 919041, result = "هذا المستخدم محذوف مسبقا" });
                }

                user.Status = Status.Deleted;
                user.ModifiedBy = UserId();
                user.ModifiedOn = DateTime.Now;
                await db.SaveChangesAsync();

                return Ok(new { statusCode = 0, result = new { result = "تم حذف المستخدم بنجاح" } });
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "API ==> Users Controller - DeleteUser  /" +
                                $"UserId ==> {UserId()} /" +
                                "Error Code ==> [919042] /" +
                                $"Paramters ==> UserId = {userId}");
                return StatusCode(500, new { statusCode = 919042, result = errorMsg });
            }
        }


        [HttpGet("GetPermissions")]
        public async Task<IActionResult> GetPermissions()
        {
            try
            {

                if (UserId() <= 0)
                {
                    return BadRequest(new { statusCode = 919038, result = "الرجاء التأكد من البيانات " });
                }

                var AllPermissions = await (from f in db.Features
                                            where f.Status != Status.Deleted
                                            && f.FeatureType == 1
                                            select new
                                            {
                                                f.FeatureId,
                                                f.FeatureName,
                                                checkAll = false,
                                                permissions = (from p in f.Permissions
                                                               orderby p.Code ascending
                                                               select new
                                                               {
                                                                   p.PermissionId,
                                                                   p.PermissionName,
                                                               }).ToList(),
                                            }).ToListAsync();



                return Ok(new { statusCode = 0, result = new { AllPermissions } });

            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "API ==> Users Controller - GetPermissions  /" +
                                       $"UserId ==> {UserId()} /" +
                                       "Error Code ==> [919048] /" +
                                       $"Paramters ==> UserId = {UserId()}");
                return StatusCode(500, new { statusCode = 919048, Message = errorMsg });
            }
        }



    }
}
