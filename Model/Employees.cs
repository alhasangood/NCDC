using System;
using System.Collections.Generic;

namespace Models
{
    public partial class Employees
    {
        public Employees()
        {
            CitzensAnalysisCreatedByNavigation = new HashSet<CitzensAnalysis>();
            CitzensAnalysisModifiedByNavigation = new HashSet<CitzensAnalysis>();
            EmployeePermissions = new HashSet<EmployeePermissions>();
        }

        public long EmployeeId { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Signature { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string RememberToken { get; set; }
        public int LoginTryAttempts { get; set; }
        public DateTime? LastLoginOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public short Status { get; set; }

        public virtual Users CreatedByNavigation { get; set; }
        public virtual Users ModifiedByNavigation { get; set; }
        public virtual ICollection<CitzensAnalysis> CitzensAnalysisCreatedByNavigation { get; set; }
        public virtual ICollection<CitzensAnalysis> CitzensAnalysisModifiedByNavigation { get; set; }
        public virtual ICollection<EmployeePermissions> EmployeePermissions { get; set; }
    }
}
