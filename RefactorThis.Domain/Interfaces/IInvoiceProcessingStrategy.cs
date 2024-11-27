using RefactorThis.Domain.Models;

namespace RefactorThis.Domain.Interfaces
{
    public interface IInvoiceProcessingStrategy
    {
        string ProcessPayment(Invoice invoice, Payment payment);
    }
}
