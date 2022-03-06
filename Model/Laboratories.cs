using System;
using System.Collections.Generic;

namespace Models
{
    public partial class Laboratories
    {
        public Laboratories()
        {
            Citizens = new HashSet<Citizens>();
            CitzensAnalysis = new HashSet<CitzensAnalysis>();
        }

        public int LaboratoryId { get; set; }
        public string LaboratoryName { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public short Status { get; set; }

        public virtual Users CreatedByNavigation { get; set; }
        public virtual Users ModifiedByNavigation { get; set; }
        public virtual ICollection<Citizens> Citizens { get; set; }
        public virtual ICollection<CitzensAnalysis> CitzensAnalysis { get; set; }
    }
}
