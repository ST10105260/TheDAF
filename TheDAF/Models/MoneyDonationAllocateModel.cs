namespace TheDAF.Models
{
    public class MoneyDonationAllocateModel
    {
        public int MoneyDonationAllocationID { get; set; }
        public int DisasterID { get; set; } // foreign key (PK in Disasters table)
        public string Description { get; set; } // from Disasters table
        public decimal Amount { get; set; }
    }
}
