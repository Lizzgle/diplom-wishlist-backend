namespace SecretSanta.Presentation.Models.Games;

public class CreateGameRequestModel
{
    public required string Name { get; set; }
    
    public string? Description { get; set; }
    
    public DateTimeOffset DateOfGeneration { get; set; }
    
    public int? MinAdvance { get; set; }
    
    public int? MaxAdvance { get; set; }
}