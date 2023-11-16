namespace SeboOnline.Models;

public class Transaction
{
    public int Id { get; set; }
    public int IdBuyer { get; set; }
    public int IdSeller { get; set; }
    public int IdItem { get; set; }
    public DateTime Date { get; set; }
    public decimal Value { get; set; }
    public User Buyer { get; set; }
    public User Seller { get; set; }
    public Item Item { get; set; }
}
