using System;
using System.Collections.Generic;

namespace Contractors.Data.DTOs
{
    public class OfferDto
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public float PrizeProposition { get; set; }
        public string Currency { get; set; }
        public string AdditionalInformation { get; set; }
        public int PrevOfferId { get; set; }
        public List<int> JobsId { get; set; }
        

    }
}
