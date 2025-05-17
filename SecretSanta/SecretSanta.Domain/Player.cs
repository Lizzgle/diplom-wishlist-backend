namespace SecretSanta.Domain;

public class Player : Entity
{
    public string Name { get; set; }
    
    public string Email { get; set; }
    
    public Guid? RecipientId { get; set; }
    public Player? Recipient { get; set; }
    
    
    //TODO добавить аватар
}