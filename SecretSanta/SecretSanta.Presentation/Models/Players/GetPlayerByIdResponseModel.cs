namespace SecretSanta.Presentation.Models.Players;

public class GetPlayerByIdResponseModel
{
    public Guid PlayerId { get; set; }
    
    public string? Card { get; set; }
    
    public Guid? RecipientId { get; set; }
}