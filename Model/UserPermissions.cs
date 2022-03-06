using System;
using System.Collections.Generic;

namespace Models
{
    public partial class UserPermissions
    {
        public long UserId { get; set; }
        public int PermissionId { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual Users CreatedByNavigation { get; set; }
        public virtual Permissions Permission { get; set; }
        public virtual Users User { get; set; }
    }
}
