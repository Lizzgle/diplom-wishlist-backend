using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Wishlist.Infrastructure.Models;

public class WishlistDto
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    
    public required string Uri  { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public required string CreatorId { get; set; }

    public List<WishDto> Wishes { get; set; } = [];
}