namespace ISPAN.Izakaya.BLL_Service_.Dtos
{
    public class PaymentConfirmResponseDto
    {
        public string ReturnCode { get; set; }
        public string ReturnMessage { get; set; }
        public ConfirmInfo Info { get; set; }
    }

    public class ConfirmInfo
    {
        public string OrderId { get; set; }
        public long TransactionId { get; set; }
        public List<PayInfo> PayInfo { get; set; }
        public List<Package> Packages { get; set; }
    }

    public class PayInfo
    {
        public string Method { get; set; }
        public int Amount { get; set; }
        public string MaskedCreditCardNumber { get; set; }
    }
}
