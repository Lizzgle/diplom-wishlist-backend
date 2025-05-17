namespace SecretSanta.Domain;

public class Game : Entity
{
    public required string Name { get; set; }
    
    public string? Description { get; set; }
    
    public required string CreatorId { get; set; }
    
    public DateTimeOffset DateOfGeneration { get; set; }
    
    public int MinAdvance { get; set; }
    
    public int MaxAdvance { get; set; }
    
    public string? Url { get; set; }
    
    //TODO добавить аватар
}