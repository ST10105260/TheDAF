namespace TheDAF.Models
{
    public class PurchaseMadeModel
    {
        public int PurchaseMadeID { get; set; }
        public int DisasterID { get; set; }
        public string Description { get; set; }
        public string PurchasedItems { get; set; }
        public decimal TotalCost { get; set; }
    }
}
