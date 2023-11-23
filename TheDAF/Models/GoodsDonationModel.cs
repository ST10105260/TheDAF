namespace TheDAF.Models
{
    public class GoodsDonationModel
    {
        public int GoodsDonationID { get; set; }
        public DateTime Date { get; set; }
        public int NoOfItems { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public bool IsAnonymous { get; set; }
    }
}
