using System;
using System.Collections.Generic;

namespace Models
{
    public partial class CitzensAnalysis
    {
        public long CitzensAnalysisId { get; set; }
        public long CitzenId { get; set; }
        public int LaboratoryId { get; set; }
        public int AnalysisId { get; set; }
        public int? ResultTypeId { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public short Status { get; set; }

        public virtual AnalysisTypes Analysis { get; set; }
        public virtual Citizens Citzen { get; set; }
        public virtual Employees CreatedByNavigation { get; set; }
        public virtual Laboratories Laboratory { get; set; }
        public virtual Employees ModifiedByNavigation { get; set; }
        public virtual ResultTypes ResultType { get; set; }
    }
}
