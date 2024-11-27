using RefactorThis.Domain.Constants;
using RefactorThis.Domain.Interfaces;
using RefactorThis.Domain.Models;
using System;

namespace RefactorThis.Domain.Services
{
    public class InvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceService(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public string ProcessPayment(Payment payment)
        {
            if (payment == null)
                throw new InvalidOperationException(ResponseMessages.InvalidInvoiceState);

            var invoice = _invoiceRepository.GetInvoice(payment.Reference);

            if (invoice == null)
                throw new InvalidOperationException(ResponseMessages.NoMatchingInvoice);

            // Handle case where invoice is already fully paid
            if (invoice.Amount == 0 || invoice.AmountPaid == invoice.Amount)
            {
                return ResponseMessages.InvoiceWasFullPaid;
            }

            // you paid to much
            if (payment.Amount > invoice.Amount)
            {
                return ResponseMessages.PaymentGreaterThanInvoiceAmount;
            }

            // Handle case where there is no partial payment, and the payment is less than the invoice amount
            if (invoice.AmountPaid == 0 && payment.Amount < invoice.Amount)
            {
                invoice.AmountPaid += payment.Amount;
                _invoiceRepository.SaveInvoice(invoice);
                return ResponseMessages.InvoicePartiallyPaid; // Return "invoice is now partially paid"
            }

            // Handle case where the payment exceeds the remaining amount
            decimal remainingAmount = invoice.Amount - invoice.AmountPaid;
            if (payment.Amount > remainingAmount)
            {
                return ResponseMessages.PaymentGreaterThanRemaining; // Correct message for partial payments
            }

            // Process the payment logic
            invoice.AmountPaid += payment.Amount;
            _invoiceRepository.SaveInvoice(invoice);

            if (invoice.AmountPaid == invoice.Amount)
            {
                return ResponseMessages.InvoiceFullyPaid;
            }
            else
            {
                return ResponseMessages.PartialPaymentReceived;
            }
        }




    }
}
