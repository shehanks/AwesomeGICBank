using AutoMapper;
using AwesomeGICBank.Application.Dtos;
using AwesomeGICBank.Core.Entities;

namespace AwesomeGICBank.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateTransactionRequest, Transaction>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.CreationDate));

            CreateMap<CreateInterestRuleRequest, InterestRule>()
                .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.RatePercentage))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.CreationDate));

            CreateMap<Transaction, TransactionDto>()
                .ForMember(dest => dest.TxnId, opt => opt.MapFrom(src => $"{src.Date:yyyyMMdd}-{src.TxnId:D2}"));

            CreateMap<InterestRule, InterestRuleDto>()
                .ForMember(dest => dest.InterestRate, opt => opt.MapFrom(src => src.Rate));
        }
    }
}
