namespace SecretSanta.Application.Usecases.Games.Queries.GetAllForUser;

public class GetAllGamesForUserResponse
{
    public List<GameDto> Games { get; set; } = new List<GameDto>();
}

public class GameDto
{
    public Guid Id { get; set; }
    
    public required string Name { get; set; }
    
    public required string CreatorId { get; set; }
    
    public DateTimeOffset DateOfGeneration { get; set; }
}