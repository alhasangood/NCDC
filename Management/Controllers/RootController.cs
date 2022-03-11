using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 using Common;


namespace Managment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RootController : ControllerBase
    {
        private Models.NCDCContext db;
        public RootController(NCDCContext context)
        {
            db = context;
        }
        public int UserId()
        {
            var claims = HttpContext.User.Claims.ToList();
            int userId = Convert.ToInt32(claims.Where(p => p.Type == "UserId").Select(p => p.Value).SingleOrDefault());
            return userId;
        }
        public bool GetUserPermission(string permissionCode)
        {
            try
            {
                var userPermisions = (from p in db.UserPermissions
                                      where p.UserId == UserId()
                                      && p.Permission.Code == permissionCode
                                      select p).Count();

                if (userPermisions <= 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public readonly string errorMsg = "The server could not process the request, please try again later or contact technical support ";
        public readonly string noPermission = "You do not have the necessary permissions to execute an request ";
        public readonly string verifyData = "Please verify the data";
        public readonly string enteredData = "Please check the entered data";
        public readonly string itemSelection = "Please check item selection";
        public readonly string notFound = "No data was found for the selected item";


        public readonly string alreadyLocked = "This item is already frozen";
        public readonly string alreadyUnLocked = "This item is already been Actived";
        public readonly string alreadyDeleteded = "This item is already been deleted";


        public readonly string Added = "Added was successful";
        public readonly string edited = "Edited was successful";
        public readonly string locked = "Freeze was successful";
        public readonly string unLocked = "Actived was successful";
        public readonly string deleteded = "Deleteded was successful";


        public readonly string errorOccurred = "An error occurred during storage";
    }

}
