using System;
using System.Collections.Generic;
using System.Text;

namespace Contractors.Data.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
    }
}
