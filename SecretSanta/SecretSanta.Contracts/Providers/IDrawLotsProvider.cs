namespace SecretSanta.Contracts.Providers;

public interface IDrawLotsProvider
{
    /// <summary>
    /// Генерирует пары участник-получатель.
    /// </summary>
    /// <param name="participantIds">Список идентификаторов участников</param>
    /// <returns>Словарь, где ключ — даритель, а значение — получатель</returns>
    Dictionary<Guid, Guid> DrawLots(List<Guid> participantIds);
}