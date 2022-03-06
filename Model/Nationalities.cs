using System;
using System.Collections.Generic;

namespace Models
{
    public partial class Nationalities
    {
        public Nationalities()
        {
            Citizens = new HashSet<Citizens>();
        }

        public long NationalitiyId { get; set; }
        public string NationalitiyNme { get; set; }
        public string CountryName { get; set; }
        public short Status { get; set; }

        public virtual ICollection<Citizens> Citizens { get; set; }
    }
}
