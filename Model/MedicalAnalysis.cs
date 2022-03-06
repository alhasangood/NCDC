using System;
using System.Collections.Generic;

namespace Models
{
    public partial class MedicalAnalysis
    {
        public int AnalysisTypeId { get; set; }
        public string AnalysisTypeName { get; set; }
        public string AnalysisTypeCode { get; set; }
        public string Description { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public short Status { get; set; }

        public virtual Users CreatedByNavigation { get; set; }
        public virtual Users ModifiedByNavigation { get; set; }
    }
}
