namespace TheDAF.Models
{
    public class PublicInfoModel
    {
        public decimal TotalMonetaryDonations { get; set; }
        public int TotalGoodsReceived { get; set; }
        public List<DisasterInfoModel> ActiveDisasters { get; set; } = new List<DisasterInfoModel>();
    }

    public class DisasterInfoModel
    {
        public int DisasterID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string AidType { get; set; }
        public decimal AllocatedMoney { get; set; }
        public int AllocatedGoods { get; set; }
    }
}
