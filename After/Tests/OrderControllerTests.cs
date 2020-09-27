using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class OrderControllerTests : IClassFixture<BaseTestServerFixture>
    {
        private readonly BaseTestServerFixture _fixture;

        public OrderControllerTests(BaseTestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task OkOnValidId()
        {
            //Act
            var response = await _fixture.Client.GetAsync("/order/1");
            
            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task UnauthorizedOnWrongUserEmail()
        {
            //Act
            var response = await _fixture.Client.GetAsync("/order/2");

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task NotFoundOnNotExistsId()
        {
            //Act
            var response = await _fixture.Client.GetAsync("/order/3");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
