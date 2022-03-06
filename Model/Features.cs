using System;
using System.Collections.Generic;

namespace Models
{
    public partial class Features
    {
        public Features()
        {
            Permissions = new HashSet<Permissions>();
        }

        public int FeatureId { get; set; }
        public string FeatureName { get; set; }
        public short FeatureType { get; set; }
        public short Status { get; set; }

        public virtual ICollection<Permissions> Permissions { get; set; }
    }
}
