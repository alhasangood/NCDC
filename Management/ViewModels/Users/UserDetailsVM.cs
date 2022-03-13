using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using FluentValidation;

namespace Management.ViewModels
{
    public class UserDetailsVM
    {
        public string LoginName { get; set; }
        public string FullName { get; set; }
        public int UserType { get; set; }
        public string JobDescription { get; set; }
        public string ParentCenter { get; set; }
        public string ParentHealthCenter { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public short Status { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public List<Features> Features { get; set; }
        public List<Permissions> Permissions { get; set; }

    }
    public class Features
    {
        public int FeatureId { get; set; }
        public string FeatureName { get; set; }
        public List<Permissions> Permissions { get; set; }
    }
    public class Permissions
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        //public int FeatureId { get; set; }
        //public string FeatureName { get; set; }
        //public List<string> permissions { get; set; }
    }

}
