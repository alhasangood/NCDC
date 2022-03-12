using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.ViewModels
{
    public class CitizenData
    {
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

    }
}
