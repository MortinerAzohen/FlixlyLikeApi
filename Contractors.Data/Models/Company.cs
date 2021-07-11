using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contractors.Data.Models
{
    public class Company
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpatedOn { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAbout { get; set; }
        public bool IsActive { get; set; }
        public Contractor Contractor { get; set; }
        public Address CompanyAddress { get; set; }
        public IList<Job> CompanyJobs { get; set; }

    }
}
