using Microsoft.AspNetCore.Mvc;
using Portal.ViewModels;

namespace Portal.Controllers
{
    public class UsersController : RootController
    {
        public UsersController(NCDCContext db) : base(db)
        {
        }

        [HttpGet]
        [ExceptionCode("EX01001")]
        public async Task<IActionResult> Get([FromQuery] Pagination pagination, [FromQuery] UsersFilterVM filter)
        {
          //  if (!await HasPermission(UsersPermissions.List)) return BadRequest(new { statusCode = "AE01001", message = AppMessages.noPermission });

            var query = from c in db.Users.Where(c => c.Status != Status.Deleted)
                                          .WhereIf(!string.IsNullOrWhiteSpace(filter.Search), c => c.FullName.Contains(filter.Search!))                   
                                          .WhereIf(filter.UserTypeId > 0, c => c.UserTypeId.Equals(filter.UserTypeId))                   
                        orderby c.UserId descending
                        select new
                        {
                            c.UserId,
                            c.FullName,
                            c.LoginName,
                           CreatedBy = c.CreatedByNavigation.FullName,
                            CreatedOn = c.CreatedOn.ToString("yyyy-MM-dd"),
                            c.Status
                        };

            var totalItems = await query.CountAsync();
            var users = await query.Skip(pagination.PageSize * (pagination.Page - 1)).Take(pagination.PageSize).ToListAsync();

            var result = new { statusCode = 1, users, totalItems };

            return Ok(result);
        }

        [HttpGet("{id}/details")]
        [ExceptionCode("EX01002")]
        public async Task<IActionResult> GetDetails(int id)
        {
           // if (!await HasPermission(UsersPermissions.ShowDetails)) return BadRequest(new { statusCode = "AE01002", message = AppMessages.noPermission });

            var user = await (from c in db.Users
                              where c.UserId == id && c.Status != Status.Deleted
                              select new
                              {
                                  c.UserId,
                                  c.FullName,
                                  c.LoginName,
                                  c.Email,
                                  CreatedBy = c.CreatedByNavigation.FullName,
                                  CreatedOn = c.CreatedOn.ToString("yyyy-MM-dd"),
                                  UpdatedBy = c.ModifiedByNavigation!.FullName,
                                  UpdatedOn = c.ModifiedOn!.Value.ToString("yyyy-MM-dd"),
                                  c.Status,
                                  Permissions = (from f in db.Features
                                                 where f.Status != Status.Deleted
                                                 select new
                                                 {
                                                     f.FeatureId,
                                                     f.FeatureName,
                                                     permissions = (from a in db.UserPermissions
                                                                    where a.UserId == id
                                                                    && a.Permission.FeatureId == f.FeatureId
                                                                    select a.Permission.PermissionName).ToList()
                                                 }).Where(x => x.permissions.Count > 0).ToList(),
                              }).SingleOrDefaultAsync();

            if (user is null)
            {
                return NotFound(new { statusCode = "RE01001", message = "لم يتم العثور على المستخدم" });
            }

            var result = new { statusCode = 1, user };

            return Ok(result);
        }

        [HttpGet("{id}")]
        [ExceptionCode("EX01003")]
        public async Task<IActionResult> Get(int id)
        {
           // if (!await HasPermission(UsersPermissions.Update)) return BadRequest(new { statusCode = "AE01003", message = AppMessages.noPermission });

            var user = await (from c in db.Users
                              where c.UserId == id && c.Status != Status.Deleted
                              select new
                              {
                                  c.UserId,
                                  c.FullName,
                                  c.LoginName,                               
                                  c.Email,
                                  Permissions = c.UserPermissionsUser.Select(x => x.PermissionId).ToList()
                              }).SingleOrDefaultAsync();

            if (user is null)
            {
                return NotFound(new { statusCode = "RE01002", message = "لم يتم العثور على المستخدم" });
            }

            var result = new { statusCode = 1, user };

            return Ok(result);
        }

        [HttpPost]
        [ExceptionCode("EX01004")]
        public async Task<IActionResult> Create([FromBody] UserVM vm)
        {
          //  if (!await HasPermission(UsersPermissions.Create)) return BadRequest(new { statusCode = "AE01004", message = AppMessages.noPermission });

            var userNameExist = await (from c in db.Users
                                       where c.LoginName.Equals(vm.LoginName) &&
                                              c.Status != Status.Deleted
                                       select c.UserId).AnyAsync();

            var emailExist = await (from c in db.Users
                                    where c.Email.Equals(vm.Email) &&
                                           c.Status != Status.Deleted
                                    select c.UserId).AnyAsync();

            if (userNameExist)
            {
                return BadRequest(new { statusCode = "RE01003", message = "اسم تسجيل الدخول موجود مسبقا" });
            }

            if (emailExist)
            {
                return BadRequest(new { statusCode = "RE01004", message = "البريد الإلكتروني موجود مسبقا" });
            }

            var newUser = new Users
            {
                LoginName = vm.LoginName,
                FullName = vm.FullName,
                Email = vm.Email,
              
                Password = Security.ComputeHash(vm.Password.Trim(), Security.Base64Decode("QXJAcUBUZWNoMjAxOCE=")),
                CreatedBy = UserId(),
                CreatedOn = DateTime.Now,
                Status = Status.Active,
            };

            vm.Permissions.ForEach(p =>
            {
                UserPermissions userPermission = new()
                {
                    PermissionId = p,
                    CreatedBy = UserId(),
                    CreatedOn = DateTime.Now,
                };

                newUser.UserPermissionsUser.Add(userPermission);
            });

            db.Add(newUser);
         

            await db.SaveChangesAsync();

            var user = await (from c in db.Users
                              where c.UserId == newUser.UserId
                              select new
                              {
                                  c.UserId,
                                  c.FullName,
                                  c.LoginName,
                                  CreatedBy = c.CreatedByNavigation.FullName,
                                  CreatedOn = c.CreatedOn.ToString("yyyy-MM-dd"),
                                  c.Status
                              }).SingleOrDefaultAsync();

            var result = new { statusCode = 1, user, message = "تم إضافة المستخدم بنجاح" };

            return Ok(result);
        }

        [HttpPut("{id}")]
        [ExceptionCode("EX01005")]
        public async Task<IActionResult> Update(int id, [FromBody] UserVM vm)
        {
           // if (!await HasPermission(UsersPermissions.Update)) return BadRequest(new { statusCode = "AE01005", message = AppMessages.noPermission });

            var userNameExist = await (from c in db.Users
                                       where c.UserId != id &&
                                             c.LoginName.Equals(vm.LoginName) &&
                                             c.Status != Status.Deleted
                                       select c).AnyAsync();

            var emailExist = await (from c in db.Users
                                    where c.UserId != id &&
                                           c.Email.Equals(vm.Email) &&
                                           c.Status != Status.Deleted
                                    select c.UserId).AnyAsync();

            if (userNameExist)
            {
                return BadRequest(new { statusCode = "RE01005", message = "اسم المستخدم موجود مسبقا" });
            }

            if (emailExist)
            {
                return BadRequest(new { statusCode = "RE01006", message = "البريد الإلكتروني موجود مسبقا" });
            }

            var userToEdit = await (from c in db.Users
                                    where c.UserId == id && c.Status != Status.Deleted
                                    select c).Include(u => u.UserPermissionsUser).SingleOrDefaultAsync();

            if (userToEdit is null)
            {
                return NotFound(new { statusCode = "RE01007", message = "لم يتم العثور على المستخدم" });
            }

            userToEdit.LoginName = vm.LoginName;
            userToEdit.FullName = vm.FullName;
            userToEdit.Email = vm.Email;
            userToEdit.ModifiedBy = UserId();
            userToEdit.ModifiedOn = DateTime.Now;

            foreach (var permission in userToEdit.UserPermissionsUser)
            {
                userToEdit.UserPermissionsUser.Remove(permission);
            }

            vm.Permissions.ForEach(p =>
            {
                UserPermissions userPermission = new()
                {
                    PermissionId = p,
                    CreatedBy = UserId(),
                    CreatedOn = DateTime.Now,
                };

                userToEdit.UserPermissionsUser.Add(userPermission);
            });



            await db.SaveChangesAsync();

            var user = await (from c in db.Users
                              where c.UserId == userToEdit.UserId
                              select new
                              {
                                  c.UserId,
                                  c.FullName,
                                  c.LoginName,                                
                                  CreatedBy = c.CreatedByNavigation.FullName,
                                  CreatedOn = c.CreatedOn.ToString("yyyy-MM-dd"),
                                  c.Status
                              }).SingleOrDefaultAsync();

            var result = new { statusCode = 1, user, message = "تم تعديل المستخدم بنجاح" };

            return Ok(result);
        }

        [HttpPut("{id}/lock")]
        [ExceptionCode("EX01006")]
        public async Task<IActionResult> Lock(int id)
        {
           // if (!await HasPermission(UsersPermissions.Lock_Unlock)) return BadRequest(new { statusCode = "AE01006", message = AppMessages.noPermission });

            var user = await (from c in db.Users
                              where c.UserId == id
                              && c.Status != Status.Deleted
                              select c).SingleOrDefaultAsync();

            if (user is null)
            {
                return NotFound(new { statusCode = "RE01008", message = "لم يتم العثور على المستخدم" });
            }

            if (user.Status == Status.Locked)
            {
                return BadRequest(new { statusCode = "RE01009", message = "هذا المستخدم مجمد مسبقا" });
            }
            if (user.Status != Status.Active)
            {
                return BadRequest(new { statusCode = "RE01010", message = "لا يمكن تجميد مخزن غير فعال" });
            }

            user.Status = Status.Locked;
            user.ModifiedBy = UserId();
            user.ModifiedOn = DateTime.Now;

            await db.SaveChangesAsync();

            var result = new { statusCode = 1, message = "تم تجميد المستخدم بنجاح" };

            return Ok(result);
        }

        [HttpPut("{id}/unlock")]
        [ExceptionCode("EX01007")]
        public async Task<IActionResult> Unlock(int id)
        {
           // if (!await HasPermission(UsersPermissions.Lock_Unlock)) return BadRequest(new { statusCode = "AE01007", message = AppMessages.noPermission });

            var user = await (from c in db.Users
                              where c.UserId == id
                              && c.Status != Status.Deleted
                              select c).SingleOrDefaultAsync();

            if (user is null)
            {
                return NotFound(new { statusCode = "RE01011", message = "لم يتم العثور على المستخدم" });
            }

            if (user.Status == Status.Active)
            {
                return BadRequest(new { statusCode = "RE01012", message = "هذا المستخدم مفعل مسبقا" });
            }

            user.Status = Status.Active;
            user.ModifiedBy = UserId();
            user.ModifiedOn = DateTime.Now;

            await db.SaveChangesAsync();

            var result = new { statusCode = 1, message = "تم فك تجميد المستخدم بنجاح" };

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ExceptionCode("EX01008")]
        public async Task<IActionResult> Delete(int id)
        {
           // if (!await HasPermission(UsersPermissions.Delete)) return BadRequest(new { statusCode = "AE01008", message = AppMessages.noPermission });

            var user = await (from c in db.Users
                              where c.UserId == id &&
                                    c.Status != Status.Deleted
                              select c).SingleOrDefaultAsync();

            if (user is null)
            {
                return NotFound(new { statusCode = "RE01013", message = "لم يتم العثور على المستخدم" });
            }

            user.Status = Status.Deleted;
            user.ModifiedBy = UserId();
            user.ModifiedOn = DateTime.Now;

            await db.SaveChangesAsync();

            var result = new { statusCode = 1, message = "تم حذف المستخدم بنجاح" };

            return Ok(result);
        }

        [HttpPut("{id}/resetPassword")]
        [ExceptionCode("EX01009")]
        public async Task<IActionResult> ResetPassword(int id, [FromBody] PasswordResetVM vm)
        {
          //  if (!await HasPermission(UsersPermissions.ResetPassword)) return BadRequest(new { statusCode = "AE01009", message = AppMessages.noPermission });

            var user = await (from c in db.Users
                              where c.UserId == id
                              select c).SingleOrDefaultAsync();

            if (user is null)
            {
                return NotFound(new { statusCode = "RE01014", message = "لم يتم العثور على المستخدم" });
            }

            user.Password = Security.ComputeHash(vm.Password.Trim(), Security.Base64Decode("QXJAcUBUZWNoMjAxOCE="));
            user.ModifiedBy = UserId();
            user.ModifiedOn = DateTime.Now;
            await db.SaveChangesAsync();

            var result = new { statusCode = 1, message = "تم إعادة تعيين كلمة مرور المستخدم بنجاح" };

            return Ok(result);
        }

        [HttpGet("getFeatures")]
        [ExceptionCode("EX01001")]
        public async Task<IActionResult> Permissions()
        {
            var userPermissions = from u in db.UserPermissions
                                  where u.UserId == UserId()
                                  select u;

            var features = await (from f in db.Features
                                  where f.Status == Status.Active
                                  select new
                                  {
                                      f.FeatureId,
                                      f.FeatureName,
                                      checkAll = false,
                                      permissions = (from p in db.Permissions
                                                     where p.FeatureId == f.FeatureId
                                                     select new
                                                     {
                                                         p.PermissionId,
                                                         p.PermissionName,
                                                     }).ToList()
                                  }).ToListAsync();

            return Ok(new { statusCode = 1, features });
        }
    }
}
