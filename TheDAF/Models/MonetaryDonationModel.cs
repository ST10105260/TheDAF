namespace TheDAF.Models
{
    public class MonetaryDonationModel
    {
        public int MonetaryDonationID { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public bool IsAnonymous { get; set; }
    }
}
