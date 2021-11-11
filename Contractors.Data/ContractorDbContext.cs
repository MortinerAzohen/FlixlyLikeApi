using Contractors.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Contractors.Data
{
    public class ContractorDbContext : IdentityDbContext<Contractor>
    {
        public ContractorDbContext()
        {

        }
        public ContractorDbContext(DbContextOptions<ContractorDbContext> options) : base(options)
        {

        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Contract> Contracts { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<JobCategory> JobCategories {get;set;}
        public virtual DbSet<Offer> Offers { get; set; }
    }
}
