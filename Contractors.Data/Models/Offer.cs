using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractors.Data.Models
{
    public class Offer
    {
        public int Id { get; set; }
        public Contractor Customer { get; set; }
        public Company SellerCompany { get; set; }
        public List<Job> ChoosenJobs { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public float PrizeProposition {get; set;}
        public string Currency { get; set; }
        public string AdditionalInformation { get; set; }
        public bool IsAcceptedByCustomer { get; set; }
        public bool IsAcceptedByCompany { get; set; }
        public bool IsActive { get; set; }
        public int ActualOfferId { get; set; }
    }
}
