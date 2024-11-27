using RefactorThis.Domain.Models;

namespace RefactorThis.Domain.Interfaces
{
    interface IInvoiceService
    {
        string ProcessPayment(Payment payment);
    }
}   
