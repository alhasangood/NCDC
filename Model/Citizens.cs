using System;
using System.Collections.Generic;

namespace Models
{
    public partial class Citizens
    {
        public Citizens()
        {
            CitzensAnalysis = new HashSet<CitzensAnalysis>();
        }

        public long CitizenId { get; set; }
        public string RegistrationNo { get; set; }
        public int LaboratoryId { get; set; }
        public string FullName { get; set; }
        public string NationalNo { get; set; }
        public DateTime BirthDate { get; set; }
        public short Gender { get; set; }
        public long NationalityId { get; set; }
        public string MotherName { get; set; }
        public short? IdentityType { get; set; }
        public string PassportNo { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public string EmployeePlace { get; set; }
        public string Employee { get; set; }
        public int? OccupationId { get; set; }
        public string PersonPhoto { get; set; }
        public string FamilyStatus { get; set; }
        public byte[] RegistryNo { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public short Status { get; set; }

        public virtual Users CreatedByNavigation { get; set; }
        public virtual Laboratories Laboratory { get; set; }
        public virtual Users ModifiedByNavigation { get; set; }
        public virtual Nationalities Nationality { get; set; }
        public virtual Occupations Occupation { get; set; }
        public virtual ICollection<CitzensAnalysis> CitzensAnalysis { get; set; }
    }
}
