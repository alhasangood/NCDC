using System;
using System.Collections.Generic;

namespace Models
{
    public partial class EmployeePermissions
    {
        public long EmployeeId { get; set; }
        public int PermissionId { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual Users CreatedByNavigation { get; set; }
        public virtual Employees Employee { get; set; }
        public virtual Permissions Permission { get; set; }
    }
}
