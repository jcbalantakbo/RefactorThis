using Moq;
using RefactorThis.Domain.Interfaces;
using RefactorThis.Domain.Models;
using RefactorThis.Domain.Services;

namespace RefactorThis.Domain.Tests
{
    [TestFixture]
    public class InvoicePaymentProcessorTests
    {
        private Mock<IInvoiceRepository> _mockRepo;
        private InvoiceService _invoiceService;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IInvoiceRepository>();
            _invoiceService = new InvoiceService(_mockRepo.Object);
        }

        [Test]
        public void ProcessPayment_Should_ThrowException_When_NoInvoiceFoundForPaymentReference()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetInvoice(It.IsAny<string>())).Returns((Invoice)null);

            var payment = new Payment(); // No reference set as in the original code

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() =>
                _invoiceService.ProcessPayment(payment)
            );

            Assert.AreEqual("There is no invoice matching this payment", ex.Message);
        }

        [Test]
        public void ProcessPayment_Should_ReturnFailureMessage_When_NoPaymentNeeded()
        {

            // Arrange
            var invoice = new Invoice
            {
                Amount = 0,
                AmountPaid = 0,
                Payments = new List<Payment>()
            };
            _mockRepo.Setup(repo => repo.GetInvoice(It.IsAny<string>())).Returns(invoice);

            var payment = new Payment(); // No reference set as in the original code

            // Act
            var result = _invoiceService.ProcessPayment(payment);

            // Assert
            Assert.AreEqual("invoice was already fully paid", result);
        }

        [Test]
        public void ProcessPayment_Should_ReturnFailureMessage_When_InvoiceAlreadyFullyPaid()
        {
            // Arrange
            var invoice = new Invoice
            {
                Amount = 10,
                AmountPaid = 10,
                Payments = new List<Payment>
                {
                    new Payment { Amount = 10 }
                }
            };
            _mockRepo.Setup(repo => repo.GetInvoice(It.IsAny<string>())).Returns(invoice);

            var payment = new Payment(); // No reference set as in the original code

            // Act
            var result = _invoiceService.ProcessPayment(payment);

            // Assert
            Assert.AreEqual("invoice was already fully paid", result);

        }

        [Test]
        public void ProcessPayment_Should_ReturnFailureMessage_When_PartialPaymentExistsAndAmountPaidExceedsAmountDue()
        {
            // Arrange
            var invoice = new Invoice
            {
                Amount = 10,
                AmountPaid = 5,
                Payments = new List<Payment>
                {
                    new Payment { Amount = 5 }
                }
            };
            _mockRepo.Setup(repo => repo.GetInvoice(It.IsAny<string>())).Returns(invoice);

            var payment = new Payment { Amount = 6 }; // No reference set as in the original code

            // Act
            var result = _invoiceService.ProcessPayment(payment);

            // Assert
            Assert.AreEqual("the payment is greater than the partial amount remaining", result);
        }

        [Test]
        public void ProcessPayment_Should_ReturnFailureMessage_When_NoPartialPaymentExistsAndAmountPaidExceedsInvoiceAmount()
        {
            // Arrange
            var invoice = new Invoice
            {
                Amount = 5,
                AmountPaid = 0,
                Payments = new List<Payment>()
            };
            _mockRepo.Setup(repo => repo.GetInvoice(It.IsAny<string>())).Returns(invoice);

            var payment = new Payment { Amount = 6 }; // No reference set as in the original code

            // Act
            var result = _invoiceService.ProcessPayment(payment);

            // Assert
            Assert.AreEqual("the payment is greater than the invoice amount", result);
        }

        [Test]
        public void ProcessPayment_Should_ReturnFullyPaidMessage_When_PartialPaymentExistsAndAmountPaidEqualsAmountDue()
        {
            // Arrange
            var invoice = new Invoice
            {
                Amount = 10,
                AmountPaid = 5,
                Payments = new List<Payment>
                {
                    new Payment { Amount = 5 }
                }
            };
            _mockRepo.Setup(repo => repo.GetInvoice(It.IsAny<string>())).Returns(invoice);

            var payment = new Payment { Amount = 5 }; // No reference set as in the original code

            // Act
            var result = _invoiceService.ProcessPayment(payment);

            // Assert
            Assert.AreEqual("invoice is now fully paid", result);
        }

        [Test]
        public void ProcessPayment_Should_ReturnFullyPaidMessage_When_NoPartialPaymentExistsAndAmountPaidEqualsInvoiceAmount()
        {
            // Arrange
            var invoice = new Invoice
            {
                Amount = 10,
                AmountPaid = 0,
                Payments = new List<Payment>
                {
                    new Payment { Amount = 10 }
                }
            };
            _mockRepo.Setup(repo => repo.GetInvoice(It.IsAny<string>())).Returns(invoice);

            var payment = new Payment { Amount = 10 }; // No reference set as in the original code

            // Act
            var result = _invoiceService.ProcessPayment(payment);

            // Assert
            Assert.AreEqual("invoice is now fully paid", result);
        }

        [Test]
        public void ProcessPayment_Should_ReturnPartiallyPaidMessage_When_PartialPaymentExistsAndAmountPaidIsLessThanAmountDue()
        {
            // Arrange
            var invoice = new Invoice
            {
                Amount = 10,
                AmountPaid = 5,
                Payments = new List<Payment>
                {
                    new Payment { Amount = 5 }
                }
            };
            _mockRepo.Setup(repo => repo.GetInvoice(It.IsAny<string>())).Returns(invoice);

            var payment = new Payment { Amount = 1 }; // No reference set as in the original code

            // Act
            var result = _invoiceService.ProcessPayment(payment);

            // Assert
            Assert.AreEqual("another partial payment received, still not fully paid", result);
        }

        [Test]
        public void ProcessPayment_Should_ReturnPartiallyPaidMessage_When_NoPartialPaymentExistsAndAmountPaidIsLessThanInvoiceAmount()
        {
            // Arrange
            var invoice = new Invoice
            {
                Amount = 10,
                AmountPaid = 0,
                Payments = new List<Payment>()
            };
            _mockRepo.Setup(repo => repo.GetInvoice(It.IsAny<string>())).Returns(invoice);

            var payment = new Payment { Amount = 1 }; // No reference set as in the original code

            // Act
            var result = _invoiceService.ProcessPayment(payment);

            // Assert
            Assert.AreEqual("invoice is now partially paid", result);
        }
    }
}
