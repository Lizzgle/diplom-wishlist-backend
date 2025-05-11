namespace Wishlist.Domain;

public class Wishlist
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public required string Uri  { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public required string CreatorId { get; set; }

    public List<Wish> Wishes { get; set; } = [];
    
    // TODO добавить иконку
}