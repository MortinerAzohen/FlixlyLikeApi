using Contractors.Data.DTOs;
using Contractors.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractors.Services
{
    public interface IContractService
    {
        public Task<BaseReturnModel<OfferDto>> CreateOffer(OfferDto offerDto);
        public Task<BaseReturnModel<OfferDto>> CreateReplayForOffer(OfferDto offerDto);
        public Task<BaseReturnModel<Offer>> AcceptOffer(string userId, int offerId);
        public Task<BaseReturnModel<Offer>> GetOffer(int OfferId);
        public Task<BaseReturnModel<List<Offer>>> GetMyOffers(string userId);
        public Task<BaseReturnModel<Contract>> GetContract(int ContractId);
        public Task<BaseReturnModel<List<Contract>>> GetMyContracts(string userId);
        public Task<BaseReturnModel<List<Offer>>> GetHistoryOfContract(int ContractId);

    }
}
