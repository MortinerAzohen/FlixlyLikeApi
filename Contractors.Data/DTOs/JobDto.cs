using Contractors.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contractors.Data.DTOs
{
    public class JobDto
    {
        public int Id { get; set; }
        public string JobName { get; set; }
        public string JobAbout { get; set; }
        public float JobPricing { get; set; }
        public string JobPricingCurrency { get; set; }
        public string JobDuration { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int JobCategoryId { get; set; }
        public string JobCategoryName { get; set; }
    }
}
