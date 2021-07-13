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
        public Task<BaseReturnModel<Offer>> AcceptOffer(bool IsOfferAccepted);
        public Task<BaseReturnModel<Offer>> GetOffer(int OfferId);
        public Task<List<BaseReturnModel<Offer>>> GetMyOffers();
        public Task<BaseReturnModel<Contract>> GetContract(int ContractId);
        public Task<List<BaseReturnModel<Contract>>> GetMyContracts();
        public Task<List<BaseReturnModel<Offer>>> GetHistoryOfContract(int ContractId);

    }
}
