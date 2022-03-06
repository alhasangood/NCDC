using System;
using System.Collections.Generic;

namespace Models
{
    public partial class Occupations
    {
        public Occupations()
        {
            Citizens = new HashSet<Citizens>();
        }

        public int OccupationId { get; set; }
        public string OccupationName { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public short Status { get; set; }

        public virtual Users CreatedByNavigation { get; set; }
        public virtual Users ModifiedByNavigation { get; set; }
        public virtual ICollection<Citizens> Citizens { get; set; }
    }
}
