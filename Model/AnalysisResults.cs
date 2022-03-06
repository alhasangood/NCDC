using System;
using System.Collections.Generic;

namespace Models
{
    public partial class AnalysisResults
    {
        public int ResultTypeId { get; set; }
        public int AnalysisTypeId { get; set; }
        public short Status { get; set; }

        public virtual AnalysisTypes AnalysisType { get; set; }
        public virtual ResultTypes ResultType { get; set; }
    }
}
