using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Wishlist.Domain;
using Wishlist.Domain.Enums;

namespace Wishlist.Infrastructure.Models;

[BsonIgnoreExtraElements]
public class WishDto
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public WishStatus Status { get; set; }

    public int Rating { get; set; }
    
    public bool IsBooked { get; set; }
    
    public string? BookedBy { get; set; }
    
    public List<Link> Links { get; set; }
    
    public FileData File { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public Guid WishlistId { get; set; }
    
    public string CreatorId { get; set; }
}