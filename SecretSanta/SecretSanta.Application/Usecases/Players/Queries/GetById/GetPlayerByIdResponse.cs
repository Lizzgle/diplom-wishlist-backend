namespace SecretSanta.Application.Usecases.Players.Queries.GetById;

public class GetPlayerByIdResponse
{
    public Guid PlayerId { get; set; }
    
    public string? Card { get; set; }
    
    public Guid? RecipientId { get; set; }
}