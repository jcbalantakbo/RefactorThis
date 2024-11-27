using RefactorThis.Domain.Constants;
using RefactorThis.Domain.Interfaces;
using RefactorThis.Domain.Models;
using System;

namespace RefactorThis.Domain
{
    public class CommercialInvoiceProcessingStrategy : IInvoiceProcessingStrategy
    {
        public string ProcessPayment(Invoice invoice, Payment payment)
        {
            if (payment.Amount <= 0)
                throw new InvalidOperationException(ResponseMessages.InvalidInvoiceState);

            invoice.AmountPaid += payment.Amount;
            invoice.TaxAmount += payment.Amount * 0.14m;

            if (invoice.AmountPaid == invoice.Amount)
                return ResponseMessages.InvoiceFullyPaid;

            return ResponseMessages.InvoicePartiallyPaid;
        }
    }
}
