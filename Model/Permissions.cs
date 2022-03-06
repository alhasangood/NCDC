using System;
using System.Collections.Generic;

namespace Models
{
    public partial class Permissions
    {
        public Permissions()
        {
            EmployeePermissions = new HashSet<EmployeePermissions>();
            UserPermissions = new HashSet<UserPermissions>();
        }

        public int PermissionId { get; set; }
        public int FeatureId { get; set; }
        public string PermissionName { get; set; }
        public string Code { get; set; }
        public short Status { get; set; }

        public virtual Features Feature { get; set; }
        public virtual ICollection<EmployeePermissions> EmployeePermissions { get; set; }
        public virtual ICollection<UserPermissions> UserPermissions { get; set; }
    }
}
