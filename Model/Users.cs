using System;
using System.Collections.Generic;

namespace Models
{
    public partial class Users
    {
        public Users()
        {
            AnalysisTypesCreatedByNavigation = new HashSet<AnalysisTypes>();
            AnalysisTypesModifiedByNavigation = new HashSet<AnalysisTypes>();
            CitizensCreatedByNavigation = new HashSet<Citizens>();
            CitizensModifiedByNavigation = new HashSet<Citizens>();
            EmployeePermissions = new HashSet<EmployeePermissions>();
            EmployeesCreatedByNavigation = new HashSet<Employees>();
            EmployeesModifiedByNavigation = new HashSet<Employees>();
            InverseCreatedByNavigation = new HashSet<Users>();
            InverseModifiedByNavigation = new HashSet<Users>();
            LaboratoriesCreatedByNavigation = new HashSet<Laboratories>();
            LaboratoriesModifiedByNavigation = new HashSet<Laboratories>();
            MedicalAnalysisCreatedByNavigation = new HashSet<MedicalAnalysis>();
            MedicalAnalysisModifiedByNavigation = new HashSet<MedicalAnalysis>();
            OccupationsCreatedByNavigation = new HashSet<Occupations>();
            OccupationsModifiedByNavigation = new HashSet<Occupations>();
            ResultTypesCreatedByNavigation = new HashSet<ResultTypes>();
            ResultTypesModifiedByNavigation = new HashSet<ResultTypes>();
            UserPermissionsCreatedByNavigation = new HashSet<UserPermissions>();
            UserPermissionsUser = new HashSet<UserPermissions>();
        }

        public long UserId { get; set; }
        public short UserTypeId { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public short? Gender { get; set; }
        public short? Age { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
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
        public virtual ICollection<AnalysisTypes> AnalysisTypesCreatedByNavigation { get; set; }
        public virtual ICollection<AnalysisTypes> AnalysisTypesModifiedByNavigation { get; set; }
        public virtual ICollection<Citizens> CitizensCreatedByNavigation { get; set; }
        public virtual ICollection<Citizens> CitizensModifiedByNavigation { get; set; }
        public virtual ICollection<EmployeePermissions> EmployeePermissions { get; set; }
        public virtual ICollection<Employees> EmployeesCreatedByNavigation { get; set; }
        public virtual ICollection<Employees> EmployeesModifiedByNavigation { get; set; }
        public virtual ICollection<Users> InverseCreatedByNavigation { get; set; }
        public virtual ICollection<Users> InverseModifiedByNavigation { get; set; }
        public virtual ICollection<Laboratories> LaboratoriesCreatedByNavigation { get; set; }
        public virtual ICollection<Laboratories> LaboratoriesModifiedByNavigation { get; set; }
        public virtual ICollection<MedicalAnalysis> MedicalAnalysisCreatedByNavigation { get; set; }
        public virtual ICollection<MedicalAnalysis> MedicalAnalysisModifiedByNavigation { get; set; }
        public virtual ICollection<Occupations> OccupationsCreatedByNavigation { get; set; }
        public virtual ICollection<Occupations> OccupationsModifiedByNavigation { get; set; }
        public virtual ICollection<ResultTypes> ResultTypesCreatedByNavigation { get; set; }
        public virtual ICollection<ResultTypes> ResultTypesModifiedByNavigation { get; set; }
        public virtual ICollection<UserPermissions> UserPermissionsCreatedByNavigation { get; set; }
        public virtual ICollection<UserPermissions> UserPermissionsUser { get; set; }
    }
}
