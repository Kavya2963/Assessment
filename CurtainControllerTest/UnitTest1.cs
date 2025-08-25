using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurtainStoreManagement.Controllers;
using CurtainStoreManagement.Data;
using CurtainStoreManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace CurtainStoreManagement.Tests
{
    [TestFixture]
    public class CurtainsControllerTests
    {
        private CurtainsController _controller;
        private AppDBContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDBContext(options);

            _context.curtains.AddRange(
                new Curtain { CurtainID = 1, Name = "Velvet Red", Price = 1200.00M },
                new Curtain { CurtainID = 2, Name = "Sheer White", Price = 800.00M }
            );
            _context.SaveChanges();

            _controller = new CurtainsController(_context);
        }

        [Test]
        public async Task GetCurtains_ReturnsAllCurtains()
        {
            var result = await _controller.GetCurtains();
            var curtains = result.Value;
            Assert.That(curtains, Is.TypeOf<List<Curtain>>());
            Assert.That(curtains.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetCurtain_ValidId_ReturnsCurtain()
        {
            var result = await _controller.GetCurtain(1);
            var curtain = result.Value;
            Assert.That(curtain, Is.Not.Null);
            Assert.That(curtain.Name, Is.EqualTo("Velvet Red"));
        }

        [Test]
        public async Task GetCurtain_InvalidId_ReturnsNotFound()
        {
            var result = await _controller.GetCurtain(999);
            Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task PostCurtain_ValidCurtain_ReturnsCreatedCurtain()
        {
            var newCurtain = new Curtain { CurtainID = 3, Name = "Linen Grey", Price = 950.00M };
            var result = await _controller.PostCurtain(newCurtain);
            var createdResult = result.Result as CreatedAtActionResult;
            var curtain = createdResult.Value as Curtain;
            Assert.That(curtain.Name, Is.EqualTo("Linen Grey"));
            Assert.That(_context.curtains.Find(3), Is.Not.Null);
        }

      
        [Test]
        public async Task PutCurtain_IdMismatch_ReturnsBadRequest()
        {
            var curtain = new Curtain { CurtainID = 99, Name = "Mismatch", Price = 500.00M };
            var result = await _controller.PutCurtain(1, curtain);
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task PutCurtain_CurtainNotFound_ReturnsNotFound()
        {
            var curtain = new Curtain { CurtainID = 999, Name = "Ghost Curtain", Price = 1000.00M };
            var result = await _controller.PutCurtain(999, curtain);
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task DeleteCurtain_ValidId_RemovesCurtain()
        {
            var result = await _controller.DeleteCurtain(1);
            Assert.That(result, Is.InstanceOf<NoContentResult>());
            Assert.That(_context.curtains.Find(1), Is.Null);
        }

        [Test]
        public async Task DeleteCurtain_InvalidId_ReturnsNotFound()
        {
            var result = await _controller.DeleteCurtain(999);
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }


        [Test]
        public async Task GetCurtains_WhenEmpty_ReturnsEmptyList()
        {
            _context.curtains.RemoveRange(_context.curtains);
            await _context.SaveChangesAsync();

            var result = await _controller.GetCurtains();
            var curtains = result.Value;
            Assert.That(curtains.Count, Is.EqualTo(0));
        }

        [Test]
        public void PostCurtain_NullObject_ThrowsArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _controller.PostCurtain(null);
            });
        }

       
        
     

        [Test]
        public async Task DeleteCurtain_RemovesOnlyOneCurtain()
        {
            await _controller.DeleteCurtain(1);
            Assert.That(_context.curtains.CountAsync().Result, Is.EqualTo(1));
        }

        [Test]
        public async Task GetCurtain_ReturnsCorrectPrice()
        {
            var result = await _controller.GetCurtain(2);
            var curtain = result.Value;
            Assert.That(curtain.Price, Is.EqualTo(800.00M));
        }

        [Test]
        public async Task PostCurtain_NegativePrice_AllowsNegativeInThisImplementation()
        {
            var newCurtain = new Curtain { CurtainID = 10, Name = "Test Negative", Price = -100.00M };
            await _controller.PostCurtain(newCurtain);
            var inserted = _context.curtains.Find(10);
            Assert.That(inserted.Price, Is.EqualTo(-100.00M));
        }


        [Test]
        public async Task DeleteCurtain_AfterMultipleDeletes_ReturnsNotFoundForNonExistent()
        {
            await _controller.DeleteCurtain(1);
            var result = await _controller.DeleteCurtain(1);
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task GetCurtain_AfterDelete_ReturnsNotFound()
        {
            await _controller.DeleteCurtain(1);
            var result = await _controller.GetCurtain(1);
            Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task PostCurtain_ZeroPrice_AllowsZeroInThisImplementation()
        {
            var newCurtain = new Curtain { CurtainID = 15, Name = "Free Curtain", Price = 0M };
            await _controller.PostCurtain(newCurtain);
            var curtain = _context.curtains.Find(15);
            Assert.That(curtain.Price, Is.EqualTo(0M));
        }
        [TearDown]
        public void TearDown()
        {
            _controller = null;
            _context.Dispose();
        }
    }
}
