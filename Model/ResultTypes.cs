using System;
using System.Collections.Generic;

namespace Models
{
    public partial class ResultTypes
    {
        public ResultTypes()
        {
            AnalysisResults = new HashSet<AnalysisResults>();
            CitzensAnalysis = new HashSet<CitzensAnalysis>();
        }

        public int ResultTypeId { get; set; }
        public string ResultTypeName { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public short Status { get; set; }

        public virtual Users CreatedByNavigation { get; set; }
        public virtual Users ModifiedByNavigation { get; set; }
        public virtual ICollection<AnalysisResults> AnalysisResults { get; set; }
        public virtual ICollection<CitzensAnalysis> CitzensAnalysis { get; set; }
    }
}
