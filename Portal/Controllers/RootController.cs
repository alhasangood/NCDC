using Microsoft.AspNetCore.Mvc;
using Models;

namespace Portal.Controllers
{
    [Route("api/[controller]")]
    public class RootController : ControllerBase
    {
        protected readonly NCDCContext db;
        public RootController(NCDCContext db)
        {
            this.db = db;
        }

        [HttpGet("UserId")]
        public int UserId()
        {
            var claims = HttpContext.User.Claims.ToList();
            int userId = Convert.ToInt32(claims.Where(p => p.Type == "UserId").Select(p => p.Value).SingleOrDefault());
            return userId;
        }

        [HttpGet("UserName")]
        public string UserName()
        {
            var claims = HttpContext.User.Claims.ToList();
            string userName = claims.Where(p => p.Type == "UserName").Select(p => p.Value).SingleOrDefault();
            return userName;
        }

        [HttpGet("UserTypeId")]
        public short UserTypeId()
        {
            var claims = HttpContext.User.Claims.ToList();
            short userTypeId = Convert.ToInt16(claims.Where(p => p.Type == "UserTypeId").Select(p => p.Value).SingleOrDefault());
            return userTypeId;
        }
        [HttpGet("HasPermission")]
        public bool HasPermission(string permissionCode)
        {
            try
            {
                var hasPermission = (from p in db.UserPermissions
                                     where p.UserId == UserId()
                                     && p.Permission.Code == permissionCode
                                     select p).Any();
                return hasPermission;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }

}
