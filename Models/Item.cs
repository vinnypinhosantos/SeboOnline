using System.ComponentModel;
using System.Data;

namespace SeboOnline.Models;

public class Item
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public bool Status { get; set; }
    public User Seller { get; set; }
    public int SellerId { get; set;}
    
    public bool IsActive { get; set; }
}
