namespace RefactorThis.Domain.Constants
{
    internal static class ResponseMessages
    {
        internal const string NoPaymentNeeded = "no payment needed";
        internal const string InvoiceAlreadyFullyPaid = "invoice was already fully paid";
        internal const string PaymentGreaterThanRemaining = "the payment is greater than the partial amount remaining";
        internal const string FinalPartialPaymentReceived = "final partial payment received, invoice is now fully paid";
        internal const string PartialPaymentReceived = "another partial payment received, still not fully paid";
        internal const string PaymentGreaterThanInvoiceAmount = "the payment is greater than the invoice amount";
        internal const string InvoiceFullyPaid = "invoice is now fully paid";
        internal const string InvoiceWasFullPaid = "invoice was already fully paid";
        internal const string InvoicePartiallyPaid = "invoice is now partially paid";
        internal const string NoMatchingInvoice = "There is no invoice matching this payment";
        internal const string InvalidInvoiceState = "The invoice is in an invalid state, it has an amount of 0 and it has payments.";
    }
}
