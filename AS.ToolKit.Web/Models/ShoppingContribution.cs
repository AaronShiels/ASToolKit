namespace AS.ToolKit.Web.Models
{
    public class ShoppingContribution
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int GroupId { get; set; }
        public int PersonId { get; set; }
    }
}