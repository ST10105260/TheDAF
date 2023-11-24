using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TheDAF.Controllers;
using TheDAF.Models;

namespace TestProject1
{
    public class UnitTest1
    {
            //testing viewing public information
            [Fact]
            public void PublicInfo()
            {
                // Arrange
                var loggerMock = new Mock<ILogger<HomeController>>();
            var PublicInfoModelMock = new Mock<PublicInfoModel>();

                HomeController controller = new HomeController(loggerMock.Object);

                // Act
                IActionResult result = controller.PublicInfo();

                // Assert using xUnit's Assert
                Xunit.Assert.IsType<ViewResult>(result);

            }

        //testing viewing the remaining amount
        [Fact]
        public void RemainingAmount()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<HomeController>>();
            var RemainingAmountModelMock = new Mock<RemainingAmountModel>();

            HomeController controller = new HomeController(loggerMock.Object);

            // Act
            IActionResult result = controller.RemainingAmount();

            // Assert using xUnit's Assert
            Xunit.Assert.IsType<ViewResult>(result);

        }
    }
}