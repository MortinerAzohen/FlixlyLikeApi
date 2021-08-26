using AutoMapper;
using Contractors.Data;
using Contractors.Data.DTOs;
using Contractors.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractors.Services.ContractService
{
    public class ContractService : IContractService
    {
        private readonly ContractorDbContext _db;
        private readonly IMapper _mapper;
        private readonly UserManager<Contractor> _userManager;

        public ContractService(ContractorDbContext db, IMapper mapper, UserManager<Contractor> userManager)
        {
            _db = db;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<BaseReturnModel<Offer>> AcceptOffer(bool IsOfferAccepted, string userId, int offerId)
        {
            var baseModel = new BaseReturnModel<Offer>();
            var offer = await _db.Offers.Include(o => o.Customer)
                                        .Include(o => o.ChoosenJobs)
                                        .Include(o => o.SellerCompany)
                                        .ThenInclude(c => c.Contractor)
                                        .FirstOrDefaultAsync(o => o.Id == offerId && (o.Customer.Id == userId || o.SellerCompany.Contractor.Id == userId));
            if(offer == null)
            {
                baseModel.ErrorMessage = "Offer not exist";
                baseModel.IsCorrect = false;
                baseModel.Model = null;
                return baseModel;
            }
            else
            {
                if(offer.Customer.Id == userId)
                {
                    offer.IsAcceptedByCustomer = true;
                }
                else
                {
                    offer.IsAcceptedByCompany = true;
                }

                if(offer.IsAcceptedByCompany == true && offer.IsAcceptedByCustomer == true)
                {
                    var contract = new Contract
                    {
                        StartDate = offer.StartDate,
                        EndDate = offer.EndDate,
                        AcceptedPrice = offer.PrizeProposition,
                        Currency = offer.Currency,
                        ChoosenJobs = offer.ChoosenJobs,
                        Customer = offer.Customer,
                        SellerCompany = offer.SellerCompany,
                        AdditionalInformation = offer.AdditionalInformation,
                        Offers = await _db.Offers.Where(o => o.ActualOfferId == offer.Id).ToListAsync()
                    };
                    _db.Add(contract);
                }

                var success = await _db.SaveChangesAsync() > 0;
                if (success)
                {
                    baseModel.IsCorrect = true;
                    baseModel.Model = offer;
                    return baseModel;
                }
                else
                {
                    baseModel.ErrorMessage = "Unable to accept offer to database";
                    baseModel.IsCorrect = false;
                    baseModel.Model = null;
                    return baseModel;
                }
            }

        }

        public async Task<BaseReturnModel<OfferDto>> CreateOffer(OfferDto offerDto)
        {
            var baseModel = new BaseReturnModel<OfferDto>();
            var company = await _db.Companies.Include(c => c.Contractor)
                                             .Include(c => c.CompanyJobs)
                                             .FirstOrDefaultAsync(c => c.Id == offerDto.CompanyId);
            if (company == null)
            {
                baseModel.ErrorMessage = "Company not exist";
                baseModel.IsCorrect = false;
                baseModel.Model = null;
                return baseModel;
            }
            else if (company.Contractor.Id == offerDto.CustomerId)
            {
                baseModel.ErrorMessage = "User can't make offer to his own company";
                baseModel.IsCorrect = false;
                baseModel.Model = null;
                return baseModel;
            }
            else if(await _userManager.FindByIdAsync(offerDto.CustomerId) == null)
            {
                baseModel.ErrorMessage = "Customer doesn't exist";
                baseModel.IsCorrect = false;
                baseModel.Model = null;
                return baseModel;
            }
            else
            {
                foreach(var job in company.CompanyJobs)
                {
                    if(!offerDto.JobsId.Contains(job.Id))
                    {
                        baseModel.ErrorMessage = "This company doesn't have one of selected jobs";
                        baseModel.IsCorrect = false;
                        baseModel.Model = null;
                        return baseModel;
                    }
                }
                var offer = new Offer
                {
                    StartDate = offerDto.StartDate,
                    EndDate = offerDto.EndDate,
                    Customer = await _userManager.FindByIdAsync(offerDto.CustomerId),
                    SellerCompany = company,
                    PrizeProposition = offerDto.PrizeProposition,
                    Currency = offerDto.Currency,
                    AdditionalInformation = offerDto.AdditionalInformation,
                    IsAcceptedByCompany = false,
                    IsAcceptedByCustomer = false,
                    IsActive = true                   
                };
                _db.Add(offer);
                var success = await _db.SaveChangesAsync() > 0;
                if (success)
                {
                    baseModel.IsCorrect = true;
                    baseModel.Model = offerDto;
                    return baseModel;
                }
                else
                {
                    baseModel.ErrorMessage = "Unable to add offer to database";
                    baseModel.IsCorrect = false;
                    baseModel.Model = null;
                    return baseModel;
                }
            }


        }

        public async Task<BaseReturnModel<OfferDto>> CreateReplayForOffer(OfferDto offerDto)
        {
            var baseModel = new BaseReturnModel<OfferDto>();
            var oldOffers = await _db.Offers.Include(o => o.Customer)
                                            .Include(o => o.ChoosenJobs)
                                            .Include(o => o.SellerCompany)
                                            .ThenInclude(c=>c.CompanyJobs)
                                            .OrderBy(o=>o.Id)
                                            .Where(o => o.Id == offerDto.PrevOfferId).ToListAsync();
            var lastOffer = oldOffers.First();
            if (oldOffers == null)
            {
                baseModel.ErrorMessage = "Offer doesn't exist";
                baseModel.IsCorrect = false;
                baseModel.Model = null;
                return baseModel;
            }
            else if(lastOffer.Customer.Id != offerDto.CustomerId)
            {
                baseModel.ErrorMessage = "Wrong Customer Id. This offer has diffrent Customer";
                baseModel.IsCorrect = false;
                baseModel.Model = null;
                return baseModel;
            }
            else if (lastOffer.SellerCompany.Id != offerDto.CompanyId)
            {
                baseModel.ErrorMessage = "Wrong Company Id. This offer has diffrent Company";
                baseModel.IsCorrect = false;
                baseModel.Model = null;
                return baseModel;
            }
            else
            {
                foreach (var job in lastOffer.SellerCompany.CompanyJobs)
                {
                    if (!offerDto.JobsId.Contains(job.Id))
                    {
                        baseModel.ErrorMessage = "This company doesn't have one of selected jobs";
                        baseModel.IsCorrect = false;
                        baseModel.Model = null;
                        return baseModel;
                    }
                }
                var offer = new Offer
                {
                    StartDate = offerDto.StartDate,
                    EndDate = offerDto.EndDate,
                    Customer = await _userManager.FindByIdAsync(offerDto.CustomerId),
                    SellerCompany = lastOffer.SellerCompany,
                    PrizeProposition = offerDto.PrizeProposition,
                    Currency = offerDto.Currency,
                    AdditionalInformation = offerDto.AdditionalInformation,
                    IsAcceptedByCompany = false,
                    IsAcceptedByCustomer = false,
                    IsActive = true,
                    
                };
                _db.Add(offer);
                lastOffer.IsActive = false;
                foreach(var oldOffer in oldOffers)
                {
                    oldOffer.ActualOfferId = offer.Id;
                }
                var success = await _db.SaveChangesAsync() > 0;
                if (success)
                {
                    baseModel.IsCorrect = true;
                    baseModel.Model = offerDto;
                    return baseModel;
                }
                else
                {
                    baseModel.ErrorMessage = "Unable to add offer to database";
                    baseModel.IsCorrect = false;
                    baseModel.Model = null;
                    return baseModel;
                }
            }
        }
        

        public async Task<BaseReturnModel<Contract>> GetContract(int ContractId)
        {
            var baseModel = new BaseReturnModel<Contract>();
            var contract = await _db.Contracts.Include(c => c.Customer)
                                              .Include(c => c.SellerCompany)
                                              .FirstOrDefaultAsync(c => c.Id == ContractId);
            if (contract == null)
            {
                baseModel.ErrorMessage = "Contract with this Id doesnt exist";
                baseModel.IsCorrect = false;
                baseModel.Model = null;
                return baseModel;
            }
            else
            {
                baseModel.IsCorrect = true;
                baseModel.Model = contract;
                return baseModel;
            }


        }

        public async Task<BaseReturnModel<List<Offer>>> GetHistoryOfContract(int ContractId)
        {
            var baseModel = new BaseReturnModel<List<Offer>>();
            var contract = await _db.Contracts.Include(c => c.Offers).FirstOrDefaultAsync(c => c.Id == ContractId);
            if (contract == null)
            {
                baseModel.ErrorMessage = "Contract with this Id doesnt exist";
                baseModel.IsCorrect = false;
                baseModel.Model = null;
                return baseModel;
            }
            else
            {
                baseModel.IsCorrect = true;
                baseModel.Model = contract.Offers;
                return baseModel;
            }

        }

        public async Task<BaseReturnModel<List<Contract>>> GetMyContracts(string userId)
        {
            var baseModel = new BaseReturnModel<List<Contract>>();
            var contracts =  await _db.Contracts.Where(c => c.Customer.Id == userId || c.SellerCompany.Contractor.Id == userId)
                                                .ToListAsync();
            if (contracts == null)
            {
                baseModel.ErrorMessage = "User with this Id doesnt have contracts";
                baseModel.IsCorrect = false;
                baseModel.Model = null;
                return baseModel;
            }
            else
            {
                baseModel.IsCorrect = true;
                baseModel.Model = contracts;
                return baseModel;
            }
        }

        public async Task<BaseReturnModel<List<Offer>>> GetMyOffers(string userId)
        {
            var baseModel = new BaseReturnModel<List<Offer>>();
            var offers = await _db.Offers.Where(o => o.IsActive == true && 
                                                    (o.Customer.Id == userId ||
                                                     o.SellerCompany.Contractor.Id == userId)).ToListAsync();
            if (offers == null)
            {
                baseModel.ErrorMessage = "User with this Id doesnt have active offers";
                baseModel.IsCorrect = false;
                baseModel.Model = null;
                return baseModel;
            }
            else
            {
                baseModel.IsCorrect = true;
                baseModel.Model = offers;
                return baseModel;
            }
        }

        public async Task<BaseReturnModel<Offer>> GetOffer(int OfferId)
        {
            var baseModel = new BaseReturnModel<Offer>();
            var offer = await _db.Offers.FirstOrDefaultAsync(o=>o.Id == OfferId);
            if (offer == null)
            {
                baseModel.ErrorMessage = "Offer doesnt exist";
                baseModel.IsCorrect = false;
                baseModel.Model = null;
                return baseModel;
            }
            else
            {
                baseModel.IsCorrect = true;
                baseModel.Model = offer;
                return baseModel;
            }
        }
    }
}
