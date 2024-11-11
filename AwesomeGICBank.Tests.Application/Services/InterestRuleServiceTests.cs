using AutoMapper;
using AwesomeGICBank.Application.Contracts;
using AwesomeGICBank.Application.Dtos;
using AwesomeGICBank.Core.Contracts;
using AwesomeGICBank.Core.Entities;
using Moq;
using System.Linq.Expressions;

namespace AwesomeGICBank.Tests.Application.Services
{
    public class InterestRuleServiceTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IInterestRuleService _interestRuleService;

        public InterestRuleServiceTests()
        {
            _mapperMock = new Mock<IMapper>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _interestRuleService = new InterestRuleService(_mapperMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task CreateInterestRuleAsync_ShouldReturnMappedInterestRuleDto()
        {
            // Arrange
            var ruleDate = new DateTime(2023, 10, 25);

            var createRequest = new CreateInterestRuleRequest
            {
                CreationDate = ruleDate,
                RatePercentage = 1.56m,
                RuleId = "RULE01"
            };

            var interestRule = new InterestRule
            {
                Id = 1,
                Date = ruleDate,
                Rate = 1.56m,
                RuleId = "RULE01",
            };

            var interestRuleDto = new InterestRuleDto
            {
                Id = 1,
                Date = ruleDate,
                RuleId = "RULE01",
                InterestRate = 1.56m
            };

            _mapperMock.Setup(m => m.Map<InterestRule>(createRequest)).Returns(interestRule);
            _unitOfWorkMock.Setup(u => u.InterestRuleRepository.InsertAsync(interestRule)).ReturnsAsync(interestRule);
            _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _mapperMock.Setup(m => m.Map<InterestRuleDto>(interestRule)).Returns(interestRuleDto);

            // Act
            var result = await _interestRuleService.CreateInterestRuleAsync(createRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(interestRuleDto, result);

            _mapperMock.Verify(m => m.Map<InterestRule>(createRequest), Times.Once);
            _unitOfWorkMock.Verify(u => u.InterestRuleRepository.InsertAsync(interestRule), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
            _mapperMock.Verify(m => m.Map<InterestRuleDto>(interestRule), Times.Once);
        }

        [Fact]
        public async Task GetAllRulesAsync_ShouldReturnMappedInterestRuleDtos()
        {
            // Arrange
            var ruleDate1 = new DateTime(2023, 10, 25);
            var ruleDate2 = new DateTime(2023, 10, 27);

            var interestRules = new List<InterestRule>
            {
                new InterestRule {
                    Id = 1,
                    Date = ruleDate1,
                    Rate = 1.56m,
                    RuleId = "RULE01"
                },
                new InterestRule {
                    Id = 2,
                    Date = ruleDate2,
                    Rate = 2.45m,
                    RuleId = "RULE02"
                }
            };

            var interestRuleDtos = new List<InterestRuleDto>
            {
                new InterestRuleDto
                {
                    Id = 1,
                    Date = ruleDate1,
                    RuleId = "RULE01",
                    InterestRate = 1.56m
                },
                new InterestRuleDto
                {
                    Id = 2,
                    Date = ruleDate1,
                    RuleId = "RULE01",
                    InterestRate = 2.45m
                },
            };

            _unitOfWorkMock.Setup(u => u.InterestRuleRepository.GetAsync(
                It.IsAny<Expression<Func<InterestRule, bool>>>(),
                It.IsAny<Func<IQueryable<InterestRule>, IOrderedQueryable<InterestRule>>>(),
                null, null, string.Empty))
                .ReturnsAsync(interestRules);

            _mapperMock.Setup(m => m.Map<List<InterestRuleDto>>(interestRules)).Returns(interestRuleDtos);

            // Act
            var result = await _interestRuleService.GetAllRulesAsync();

            // Assert
            Assert.True(result?.Any() == true);
            Assert.Equal(interestRuleDtos, result);

            _unitOfWorkMock.Verify(u => u.InterestRuleRepository.GetAsync(
                It.IsAny<Expression<Func<InterestRule, bool>>>(),
                It.IsAny<Func<IQueryable<InterestRule>, IOrderedQueryable<InterestRule>>>(),
                null, null, string.Empty), Times.Once);

            _mapperMock.Verify(m => m.Map<List<InterestRuleDto>>(interestRules), Times.Once);
        }
    }
}
