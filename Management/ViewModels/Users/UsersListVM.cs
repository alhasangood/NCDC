using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using FluentValidation;

namespace Management.ViewModels
{
    public class UsersListVM
    {
        public int UserId { get; set; }
        public short UserType { get; set; }
        public string LoginName { get; set; }
        public string JobDescription { get; set; }
        public string ParentCenter { get; set; }
        public short Status { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
    }
}
