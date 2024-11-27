using RefactorThis.Domain.Enums;
using RefactorThis.Domain.Interfaces;
using System;

namespace RefactorThis.Domain
{
    public class InvoiceProcessingFactory
    {
        public IInvoiceProcessingStrategy Create(InvoiceType type)
        {
            switch (type)
            {
                case InvoiceType.Standard:
                    return new StandardInvoiceProcessingStrategy();
                case InvoiceType.Commercial:
                    return new CommercialInvoiceProcessingStrategy();
                default:
                    throw new InvalidOperationException("Unsupported invoice type.");
            }
        }
    }
}
