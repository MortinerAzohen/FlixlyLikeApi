using Contractors.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractors.Data.DTOs
{
    public class CompanyDto
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAbout { get; set; }
        public bool IsActive { get; set; }
        public Address CompanyAddress { get; set; }
        public string CompanyContractorName { get; set; }
    }
}
