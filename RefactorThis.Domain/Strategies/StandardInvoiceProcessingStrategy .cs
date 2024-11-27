using RefactorThis.Domain.Constants;
using RefactorThis.Domain.Interfaces;
using RefactorThis.Domain.Models;
using System;

namespace RefactorThis.Domain
{
    public class StandardInvoiceProcessingStrategy : IInvoiceProcessingStrategy
    {
        public string ProcessPayment(Invoice invoice, Payment payment)
        {
            if (payment.Amount > invoice.Amount - invoice.AmountPaid)
                throw new InvalidOperationException(ResponseMessages.PaymentGreaterThanInvoiceAmount);

            invoice.AmountPaid += payment.Amount;

            if (invoice.AmountPaid == invoice.Amount)
                return ResponseMessages.InvoiceFullyPaid;

            return ResponseMessages.InvoicePartiallyPaid;
        }
    }
}
