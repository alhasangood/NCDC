using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Portal.ViewModels
{
    public class UserData
    {
        public long UserId { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string JobDesc { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public List<int> SelectedPermissions { get; set; }
    }
}
