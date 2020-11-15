using AutoMapper;
using DataLayer;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ServiceLayer;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebLayer.Controllers;
using WebLayer.Viewmodels;

namespace UnitTestLayer
{
    [TestFixture]
    public class Tests
    {
        private Mock<IEventService> eventServiceMock;
        private Mock<IMapper> mapper;
        private EventsController controller;

        [OneTimeSetUp]
        public void SetupOnce()
        {
            this.eventServiceMock = new Mock<IEventService>();
            this.mapper = new Mock<IMapper>();
            this.controller = new EventsController(this.eventServiceMock.Object, mapper.Object);
        }

        [SetUp]
        public void Setup()
        {
            // tva e predi vseki test
        }

        [Test]
        [Order(2)]
        public async Task GetAll_WhenCalled_ReturnsExactNumberOfEvents()
        {
            IList<Event> fakeData = new List<Event> { new Event() };
            //eventServiceMock.Setup<Task<IList<Event>>>(x => x.GetAllAsync())
            //.Returns<Task<IList<Event>>>(x => Task.FromResult(fakeData));

            eventServiceMock.Setup(x => x.GetAllAsync())
                            .ReturnsAsync(fakeData);

            var result = await this.controller.GetAll();

            OkObjectResult okResult = result.Result as OkObjectResult;
            Assert.IsInstanceOf<IEnumerable<EventDetails>>(okResult.Value);
            var list = okResult.Value as IList<EventDetails>;
            Assert.AreEqual(1, list.Count);
        }


        [Test]
        [Order(1)]        // mojesh da si izbirash reda na testovete
        public async Task Test1()
        {
            await Task.Run(() => { });

            Assert.Pass();
        }

        [TearDown]
        public void TearDown()
        {
            // pak sled vseki test
        }

        [OneTimeTearDown]
        public void TearDownAfterAll()
        {
            // sled vsichki
        }
    }
}