using System;
using System.Collections.Generic;
using System.Text;

namespace Contractors.Data.Models
{
    public class Contract
    {
        public int Id { get; set; }
        public Contractor Customer { get; set; }
        public Company SellerCompany { get; set; }
        public List<Job> ChoosenJobs { get; set; }
        public float AcceptedPrice { get; set; }
        public string Currency { get; set; }
        public string AdditionalInformation { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Offer> Offers { get; set; }
        
    }
}
