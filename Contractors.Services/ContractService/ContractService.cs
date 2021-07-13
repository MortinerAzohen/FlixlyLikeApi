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
        public Task<BaseReturnModel<Offer>> AcceptOffer(bool IsOfferAccepted)
        {
            throw new NotImplementedException();
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
                    if(!offerDto.jobsId.Contains(job.Id))
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
            var lastOffer = await _db.Offers.Include(o => o.Customer)
                                            .Include(o => o.ChoosenJobs)
                                            .Include(o => o.SellerCompany)
                                            .ThenInclude(c=>c.CompanyJobs)
                                            .FirstOrDefaultAsync(o => o.Id == offerDto.PrevOfferId);
            if (lastOffer == null)
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
                    if (!offerDto.jobsId.Contains(job.Id))
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
                lastOffer.ActualOfferId = offer.Id;
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
        

        public Task<BaseReturnModel<Contract>> GetContract(int ContractId)
        {
            throw new NotImplementedException();
        }

        public Task<List<BaseReturnModel<Offer>>> GetHistoryOfContract(int ContractId)
        {
            throw new NotImplementedException();
        }

        public Task<List<BaseReturnModel<Contract>>> GetMyContracts()
        {
            throw new NotImplementedException();
        }

        public Task<List<BaseReturnModel<Offer>>> GetMyOffers()
        {
            throw new NotImplementedException();
        }

        public Task<BaseReturnModel<Offer>> GetOffer(int OfferId)
        {
            throw new NotImplementedException();
        }
    }
}
