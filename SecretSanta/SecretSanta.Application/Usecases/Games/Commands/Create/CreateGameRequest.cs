using MediatR;

namespace SecretSanta.Application.Usecases.Games.Commands.Create;

public class CreateGameRequest : IRequest
{
    public required string Name { get; set; }
    
    public string? Description { get; set; }
    
    public required string CreatorId { get; set; }
    
    public DateTimeOffset DateOfGeneration { get; set; }
    
    public int MinAdvance { get; set; }
    
    public int MaxAdvance { get; set; }
}