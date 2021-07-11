using System;
using System.Collections.Generic;
using System.Text;

namespace Contractors.Data.Models
{
    public class Job
    {
        public int Id { get; set; }
        public string JobName { get; set; }
        public string JobAbout { get; set; }
        public float JobPricing { get; set; }
        public string JobPricingCurrency { get; set; }
        public string JobDuration { get; set; }
        public Company Company { get; set; }
        public JobCategory JobCategory { get; set; }

    }
}
