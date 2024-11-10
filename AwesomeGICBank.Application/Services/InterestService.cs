using AutoMapper;
using AwesomeGICBank.Application.Dtos;
using AwesomeGICBank.Core.Contracts;
using AwesomeGICBank.Core.Entities;

namespace AwesomeGICBank.Application.Contracts
{
    public class InterestService : IInterestService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public InterestService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<InterestRuleDto> CreateInterestRuleAsync(CreateInterestRuleRequest createInterestRuleRequest)
        {
            var interestRule = _mapper.Map<InterestRule>(createInterestRuleRequest);
            await _unitOfWork.InterestRuleRepository.InsertAsync(interestRule);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<InterestRuleDto>(interestRule);
        }

        public async Task<List<InterestRuleDto>> GetAllRulesAsync()
        {
            var insertRules = await _unitOfWork.InterestRuleRepository
                .GetAsync(orderBy: o => o.OrderBy(d => d.Date));

            return _mapper.Map<List<InterestRuleDto>>(insertRules);
        }
    }
}
