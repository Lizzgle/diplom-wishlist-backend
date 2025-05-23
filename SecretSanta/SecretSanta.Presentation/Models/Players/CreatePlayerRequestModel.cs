namespace SecretSanta.Presentation.Models.Players;

public class CreatePlayerRequestModel
{
    public string? Card { get; set; }
    
    public Guid GameId { get; set; }
}