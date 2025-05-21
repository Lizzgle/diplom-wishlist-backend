using SecretSanta.Contracts.Providers;

namespace SecretSanta.Application.Providers;

public class DrawLotsProvider : IDrawLotsProvider
{
    private readonly Random _random = new();
    
    /// <inheritdoc/>
    public Dictionary<Guid, Guid> DrawLots(List<Guid> participantIds)
    {
        if (participantIds == null || participantIds.Count < 2)
            throw new ArgumentException("Для жеребьевки необходимо минимум два участника.");

        var givers = new List<Guid>(participantIds);
        var receivers = new List<Guid>(participantIds);
        Dictionary<Guid, Guid> result = new();

        int attempts = 0;
        const int maxAttempts = 100;

        while (attempts < maxAttempts)
        {
            Shuffle(receivers);

            bool valid = true;
            result.Clear();

            for (int i = 0; i < givers.Count; i++)
            {
                if (givers[i] == receivers[i])
                {
                    valid = false;
                    break;
                }
                result[givers[i]] = receivers[i];
            }

            if (valid)
                return result;

            attempts++;
        }

        throw new InvalidOperationException("Не удалось корректно распределить участников без совпадений после нескольких попыток.");
    }
    
    private void Shuffle(List<Guid> list)
    {
        int n = list.Count;
        for (int i = 0; i < n; i++)
        {
            int j = _random.Next(i, n);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}