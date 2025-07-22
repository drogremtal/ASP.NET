using JetBrains.Annotations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Controllers;
using PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PromoCodeFactory.UnitTests.WebHost.Controllers.Partners
{

    public class SetPartnerPromoCodeLimitAsyncTests
    {


        private readonly PartnersController _partnersController;
        private readonly Mock<IRepository<Partner>> _partnerRepositoryMock;


        public SetPartnerPromoCodeLimitAsyncTests()
        {
            _partnerRepositoryMock = new Mock<IRepository<Partner>>();
            _partnersController = new PartnersController(_partnerRepositoryMock.Object);

        }



        public SetPartnerPromoCodeLimitRequest CreateCodeLimitRequest()
        {
            return new SetPartnerPromoCodeLimitRequest()
            {
                EndDate = DateTime.Now.AddDays(14),
                Limit = 10
            };
        }


        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_PartnerIsNotFound_ReturnsNotFound()
        {
            // Arrange
            Partner partner_null = null;
            ;

            var partner = _partnerRepositoryMock.Setup(q => q.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Partner)null);

            var id = Guid.NewGuid();
            SetPartnerPromoCodeLimitRequest limitRequest = CreateCodeLimitRequest();

            //Act

            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(id, limitRequest);

            //assert
            Assert.IsType<NotFoundResult>(result);


        }

        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_PartnerIsNotActive_ReturnsBadRequest()
        {
            // Arrange
            Partner partner_test = new Partner()
            {
                IsActive = false
            };

            var partner = _partnerRepositoryMock.Setup(q => q.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(partner_test);

            var id = Guid.NewGuid();
            SetPartnerPromoCodeLimitRequest limitRequest = CreateCodeLimitRequest();

            //Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(id, limitRequest);
            //assert
            Assert.IsType<BadRequestObjectResult>(result);
        }



        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_ActiveLimitIsNotNull_NumberIssuedPromoCodesIsZero()
        {
            // Arrange
            int limit = 10;
            Partner partner_test = new Partner()
            {
                IsActive = true,
                PartnerLimits = new List<PartnerPromoCodeLimit>() { new PartnerPromoCodeLimit() { CancelDate = null } },
                NumberIssuedPromoCodes = limit
            };

            var partner = _partnerRepositoryMock.Setup(q => q.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(partner_test);

            var id = Guid.NewGuid();
            SetPartnerPromoCodeLimitRequest limitRequest = CreateCodeLimitRequest();
            limitRequest.Limit = 10;
            limitRequest.EndDate = DateTime.Now.AddDays(1);

            //Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(id, limitRequest);
            //assert
            Assert.Equal(partner_test.NumberIssuedPromoCodes, 0);
        }


        /// <summary>
        /// если лимит закончился, то количество не обнуляется;
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_ActiveLimitIsNull_NumberIssuedPromoCodesIsNum()
        {
            // Arrange

            int limit = 10;

            Partner partner_test = new Partner()
            {
                IsActive = true,
                PartnerLimits = new List<PartnerPromoCodeLimit>()
                    { new PartnerPromoCodeLimit() { CancelDate = DateTime.Now } },
                NumberIssuedPromoCodes = limit
            };

            _partnerRepositoryMock.Setup(q => q.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(partner_test);

            var id = Guid.NewGuid();
            SetPartnerPromoCodeLimitRequest limitRequest = CreateCodeLimitRequest();
            limitRequest.Limit = limit;
            limitRequest.EndDate = DateTime.Now.AddDays(1);

            //Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(id, limitRequest);
            //assert
            Assert.Equal(partner_test.NumberIssuedPromoCodes, limit);
        }

        /// <summary>
        /// При установке лимита нужно отключить предыдущий лимит;
        /// </summary>

        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_SetLimit_CancelLastLimit()
        {
            // Arrange

            int limit = 10;
            var id = Guid.NewGuid();

            Partner partner_test = new Partner()
            {
                IsActive = true,
                PartnerLimits = new List<PartnerPromoCodeLimit>() { new PartnerPromoCodeLimit() { CancelDate = null } },
                NumberIssuedPromoCodes = limit
            };

            SetPartnerPromoCodeLimitRequest limitRequest = CreateCodeLimitRequest();
            limitRequest.Limit = limit;
            limitRequest.EndDate = DateTime.Now.AddDays(1);

            _partnerRepositoryMock.Setup(q => q.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(partner_test);

            //Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(id, limitRequest);
            //assert
            Assert.NotNull(partner_test.PartnerLimits.ElementAt(0).CancelDate);
        }


        /// <summary>
        /// Лимит должен быть больше 0;
        /// </summary>
        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_LimitIsLessZero_ReturnsBadRequest()
        {
            // Arrange

            int limit = 10;
            var id = Guid.NewGuid();

            Partner partner_test = new Partner()
            {
                IsActive = true,
                PartnerLimits = new List<PartnerPromoCodeLimit>() { new PartnerPromoCodeLimit() { CancelDate = null } },
                NumberIssuedPromoCodes = limit
            };

            SetPartnerPromoCodeLimitRequest limitRequest = CreateCodeLimitRequest();
            limitRequest.Limit = 0;
            limitRequest.EndDate = DateTime.Now.AddDays(1);
            _partnerRepositoryMock.Setup(q => q.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(partner_test);

            //Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(id, limitRequest);
            //assert
            Assert.IsType<BadRequestObjectResult>(result);
        }



        /// <summary>
        ///Нужно убедиться, что сохранили новый лимит в базу данных (это нужно проверить Unit-тестом)
        /// </summary>
        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_SaveNewLimit_ReturnPartnerLimit()
        {
            // Arrange
            int limit = 10;
            var id = Guid.NewGuid();

            Partner partner_test = new Partner()
            {
                IsActive = true,
                PartnerLimits = new List<PartnerPromoCodeLimit>() { new PartnerPromoCodeLimit() { CancelDate = null } },
                NumberIssuedPromoCodes = limit
            };

            SetPartnerPromoCodeLimitRequest limitRequest = CreateCodeLimitRequest();
            limitRequest.Limit = limit;
            limitRequest.EndDate = DateTime.Now.AddDays(1);
            _partnerRepositoryMock.Setup(q => q.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(partner_test);

            //Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(id, limitRequest);
            //assert
            _partnerRepositoryMock.Verify(q=>q.UpdateAsync(partner_test),Times.Once);

        }
    }
}