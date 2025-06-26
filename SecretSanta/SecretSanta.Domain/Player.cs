namespace SecretSanta.Domain;

public class Player : Entity
{
    public required string UserId { get; set; }
    
    public string? Card { get; set; }
    
    public Guid? RecipientId { get; set; }
    public Player? Recipient { get; set; }
    
    public Guid GameId { get; set; }
    public Game? Game { get; set; }
    
    //TODO добавить аватар
}