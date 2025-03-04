using Microsoft.Extensions.Configuration;
using Moq;
using ResellioBackend.UserManagementSystem.Services.Implementations;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ResellioBackendTests.UserManagementSystemTests.ServicesTests
{
    public class SendGridEmailSenderTests
    {
        private readonly Mock<ISendGridClient> _sendGridClientMock;
        private readonly SendGridEmailSender _emailSender;

        public SendGridEmailSenderTests()
        {
            _sendGridClientMock = new Mock<ISendGridClient>();

            var inMemorySettings = new Dictionary<string, string> {
                {"SendGridParameters:SendGridKey", "fake-api-key"},
                {"SendGridParameters:CompanyEmail", "noreply@resellio.com"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _emailSender = new SendGridEmailSender(configuration)
            {
                ClientFactory = () => _sendGridClientMock.Object
            };
        }

        [Fact]
        public async Task SendGridEmailSender_SendEmailAsync_ReturnsTrue_WhenEmailSentSuccessfully()
        {
            // Arrange
            var response = new Response(HttpStatusCode.OK, null, null);
            _sendGridClientMock
                .Setup(c => c.SendEmailAsync(It.IsAny<SendGridMessage>(), default))
                .ReturnsAsync(response);

            // Act
            var result = await _emailSender.SendEmailAsync("test@example.com", "Subject", "Text", "HTML");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task SendGridEmailSender_SendEmailAsync_ReturnsFalse_WhenEmailSendingFails()
        {
            // Arrange
            var response = new Response(HttpStatusCode.BadRequest, null, null);
            _sendGridClientMock
                .Setup(c => c.SendEmailAsync(It.IsAny<SendGridMessage>(), default))
                .ReturnsAsync(response);

            // Act
            var result = await _emailSender.SendEmailAsync("test@example.com", "Subject", "Text", "HTML");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task SendGridEmailSender_SendEmailAsync_CallsSendGridClientWithCorrectParameters()
        {
            // Arrange
            var response = new Response(HttpStatusCode.OK, null, null);
            _sendGridClientMock
                .Setup(c => c.SendEmailAsync(It.IsAny<SendGridMessage>(), default))
                .ReturnsAsync(response);

            // Act
            await _emailSender.SendEmailAsync("to@example.com", "Subject", "PlainText", "HtmlContent");

            // Assert
            _sendGridClientMock.Verify(c => c.SendEmailAsync(It.Is<SendGridMessage>(msg =>
                msg.From.Email == "noreply@resellio.com" &&
                msg.Subject == "Subject" &&
                msg.Personalizations[0].Tos[0].Email == "to@example.com"
            ), default), Times.Once);
        }
    }
}
