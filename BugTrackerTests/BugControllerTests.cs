using AutoFixture;
using BugTracker2.Controllers;
using BugTracker2.Interfaces;
using BugTracker2.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BugTrackerTests
{
    public class BugControllerTests
    {
        public Fixture _fixture{ get; set; }
        private Mock<IBugRepository> _IBugRepositorymock { get; set; }
        public BugControllerTests()
        {
            _fixture = new Fixture();
            _IBugRepositorymock = new Mock<IBugRepository>();
        }
        [Fact]
        public async Task GetBugByIdAsync_ShouldReturnBugByIdAsync()
        {
            // Arrange
            var fakeBug = _fixture.Create<Bug>();
            _IBugRepositorymock.Setup(x => x.GetBugById(fakeBug.Id)).Returns(Task.FromResult(fakeBug));
            var controller = new BugController(_IBugRepositorymock.Object);
            // Act
            var result = await controller.GetAsync(fakeBug.Id);
            // Assert
            var resultObject = result as OkObjectResult;
            var bug = resultObject.Value as Bug;
            Assert.Equal(bug, fakeBug);

            // Przenieœæ logikê pracy z baz¹ danych z kontrolera do repozytorium i do kazdej metody dopisaæ unit testy.
        }
    }
}
