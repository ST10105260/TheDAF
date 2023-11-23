namespace TheDAF.Models
{
    public class DisasterModel
    {
        public int DisasterID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; } // Nullable DateTime
        public string Location { get; set; }
        public string Description { get; set; }
        public string AidType { get; set; }
        public bool Active { get; set; }
    }
}
