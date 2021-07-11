using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contractors.Data.Models
{
    public class Contractor : IdentityUser
    {
        public string ContractorAbout { get; set; }
        public Address HomeAddress { get; set; }
        public IList<Company> ContractorCompanies { get; set; }
        
    }
}
