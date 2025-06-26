namespace SecretSanta.Presentation.Models.Games;

public class GetAllGamesForUserResponseModel
{
    public List<GameDtoModel> Games { get; set; } = new List<GameDtoModel>();
}

public class GameDtoModel
{
    public Guid Id { get; set; }
    
    public required string Name { get; set; }
    
    public required string CreatorId { get; set; }
    
    public DateTimeOffset DateOfGeneration { get; set; }
}