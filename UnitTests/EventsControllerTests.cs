using AutoMapper;
using DataLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebLayer.Controllers;
using WebLayer.Viewmodels;
using Xunit;

namespace UnitTests
{
    public class EventsControllerTests
    {
        private Mock<IEventService> mockService;
        private EventsController controller;
        private Mock<IMapper> mockMapper;

        public EventsControllerTests()
        {
            this.mockService = new Mock<IEventService>();
            this.mockMapper = new Mock<IMapper>();
            this.controller = new EventsController(this.mockService.Object, this.mockMapper.Object);
        }

        [Fact]
        public async Task GetAll_WhenCalled_ReturnsExactNumberOfEvents()
        {
            var fakeData = new List<Event>() { new Event(), new Event() };

            // Arrange
            mockService.Setup(x => x.GetAllAsync())
                    .ReturnsAsync(fakeData);

            //mockService.Setup(service => service.GetAllAsync())
            //        .ReturnsAsync(Task.FromResult(expectedResult));

            // Act
            ActionResult<IList<EventDetails>> result = await this.controller.GetAll();

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result.Result);
            List<EventDetails> returnValue = Assert.IsType<List<EventDetails>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }
    }
}
