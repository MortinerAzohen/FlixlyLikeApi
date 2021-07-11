using System;
using System.Collections.Generic;
using System.Text;

namespace Contractors.Data.Models
{
    public class JobCategory
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public IList<Job> JobsUnderCategory { get; set; }
    }
}
