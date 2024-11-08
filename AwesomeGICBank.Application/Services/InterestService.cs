using AwesomeGICBank.Core.Contracts;
using AwesomeGICBank.Core.Entities;

namespace AwesomeGICBank.Application.Contracts
{
    public class InterestService : IInterestService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InterestService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateInterestRuleAsync(InterestRule interestRule)
        {
            _unitOfWork.InterestRuleRepository.Insert(interestRule);
            await _unitOfWork.CompleteAsync();
        }
    }
}
