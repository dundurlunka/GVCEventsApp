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
            // Arrange
            var fakeData = new List<Event>() { new Event(), new Event() };
            var fakeMappedData = new List<EventDetails>() { new EventDetails(), new EventDetails() };

            mockService.Setup(service => service.GetAllAsync())
                    .ReturnsAsync(fakeData);
            mockMapper.Setup(mapper => mapper.Map<IList<EventDetails>>(It.IsAny<IList<Event>>()))
                    .Returns(new List<EventDetails>() { new EventDetails(), new EventDetails() });
            // Act
            ActionResult<IList<EventDetails>> result = await this.controller.GetAll();

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result.Result);
            List<EventDetails> returnValue = Assert.IsType<List<EventDetails>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetById_WhenNonExistingElementWithId_ReturnsNotFound()
        {
            // Arrange
            int fakeId = 1;
            mockService.Setup(service => service.GetByIdAsync(fakeId))
                    .ReturnsAsync((Event)null);

            // Act
            ActionResult<EventDetails> result = await this.controller.GetById(fakeId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetById_ReturnsOkWithModel()
        {
            // Arrange
            int fakeId = 1;
            Event fakeDataFromService = new Event();
            fakeDataFromService.Id = fakeId;

            EventDetails fakeMappedModel = new EventDetails();
            fakeMappedModel.Id = fakeId;

            mockService.Setup(service => service.GetByIdAsync(fakeId))
                    .ReturnsAsync(fakeDataFromService);
            mockMapper.Setup(mapper => mapper.Map<EventDetails>(fakeDataFromService))
                    .Returns(fakeMappedModel);


            // Act
            ActionResult<EventDetails> result = await this.controller.GetById(fakeId);

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result.Result);
            EventDetails returnValue = Assert.IsType<EventDetails>(okResult.Value);
            Assert.Equal(fakeId, returnValue.Id);
        }

        [Fact]
        public async Task CreateEvent_WhenModelIsNotValid_ReturnsBadRequest()
        {
            // Arrange
            EventFormModel fakeDataFromBody = new EventFormModel();

            this.controller.ModelState.AddModelError("StartDate", "StartDate must be in the future.");

            // Act
            ActionResult<Event> result = await this.controller.CreateEvent(fakeDataFromBody);

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task CreateEvents_ReturnsCreateAtActionWithModel()
        {
            // Arrange
            EventFormModel fakeDataFromBody = new EventFormModel();
            Event fakeMappedDataFromBody = new Event();
            fakeMappedDataFromBody.Name = "Test";
            mockMapper.Setup(mapper => mapper.Map<Event>(fakeDataFromBody))
                    .Returns(fakeMappedDataFromBody);
            mockService.Setup(service => service.CreateEventAsync(fakeMappedDataFromBody));

            // Act
            ActionResult<Event> result = await this.controller.CreateEvent(fakeDataFromBody);

            // Assert
            CreatedAtActionResult createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("GetById", createdAtActionResult.ActionName);
            Assert.True(createdAtActionResult.RouteValues.ContainsKey("id"));

            Event returnValue = Assert.IsType<Event>(createdAtActionResult.Value);
            Assert.Equal("Test", returnValue.Name);
        }

        [Fact]
        public async Task UpdateEvent_WhenModelIsNotValid_ReturnsBadRequest()
        {
            // Arrange
            EventFormModel fakeDataFromBody = new EventFormModel();
            int fakeId = 1;

            this.controller.ModelState.AddModelError("StartDate", "StartDate must be in the future.");

            // Act
            ActionResult<Event> result = await this.controller.UpdateEvent(fakeId, fakeDataFromBody);

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task UpdateEvent_WhenNoExistingEntityWithId_ReturnsNotFound()
        {
            // Arrange
            EventFormModel fakeDataFromBody = new EventFormModel();
            int fakeId = 1;

            mockService.Setup(service => service.GetByIdAsync(fakeId))
                    .ReturnsAsync((Event)null);

            // Act
            ActionResult<Event> result = await this.controller.UpdateEvent(fakeId, fakeDataFromBody);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task UpdateEvent_ReturnsOkWithModel()
        {
            // Arrange
            int fakeId = 1;
            EventFormModel fakeDataFromBody = new EventFormModel();
            Event fakeDataFromService = new Event();

            mockService.Setup(service => service.GetByIdAsync(fakeId))
                    .ReturnsAsync(fakeDataFromService);
            mockMapper.Setup(mapper => mapper.Map(fakeDataFromBody, fakeDataFromService));
            mockService.Setup(service => service.UpdateEventAsync(fakeDataFromService));
            fakeDataFromService.Name = "Test";

            // Act
            ActionResult<Event> result = await this.controller.UpdateEvent(fakeId, fakeDataFromBody);

            // Assert
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            Event returnValue = Assert.IsType<Event>(okObjectResult.Value);

            Assert.Equal("Test", returnValue.Name);
        }

        [Fact]
        public async Task DeleteEvent_WhenNoExistingEntityWithId_ReturnsNotFound()
        {
            // Arrange
            int fakeId = 1;
            mockService.Setup(service => service.GetByIdAsync(fakeId))
                    .ReturnsAsync((Event)null);

            // Act
            ActionResult result = await this.controller.DeleteEvent(fakeId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteEvent_ReturnsNoContent()
        {
            // Arrange
            int fakeId = 1;
            Event fakeEventFromService = new Event();

            mockService.Setup(service => service.GetByIdAsync(fakeId))
                    .ReturnsAsync(fakeEventFromService);
            mockService.Setup(service => service.DeleteEventAsync(fakeEventFromService));

            // Act
            ActionResult result = await this.controller.DeleteEvent(fakeId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
