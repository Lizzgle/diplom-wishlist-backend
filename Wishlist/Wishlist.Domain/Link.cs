namespace Wishlist.Domain;

public class Link
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public required string Uri  { get; set; }
    
    public decimal Price { get; set; }
}