namespace TheDAF.Models
{
    public class GoodsDonationAllocationModel
    {
        public int GoodsDonationAllocationID { get; set; }
        public int DisasterID { get; set; } // foreign key (PK in Disasters table)
        public string Description { get; set; } // from Disasters table
        public string Description_Goods { get; set; }
        public string NoOfItems { get; set; }
    }
}
