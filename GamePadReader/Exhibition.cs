namespace GamePadReader;

public class Exhibition
{
    public string Title { get; set; }
    public List<Session> Sessions { get; }
}

public class Session
{
    public DateTime Start { get; }
    
    public DateTime End { get; }

    public int NumberOfPrizeDraws { get; }

    public List<DateTime> PrizesDrawn { get; } = new();

    public Session(DateTime start, DateTime end, int numberOfPrizeDraws)
    {
        if (start > end)
        {
            throw new ArgumentException("Start date must be before end date");
        }
        
        Start = start;
        End = end;
        NumberOfPrizeDraws = numberOfPrizeDraws;
    }


    public double GetChanceToWin(DateTime dt)
    {
        if (dt < Start 
            || dt >= End
            || PrizesDrawn.Count >= NumberOfPrizeDraws)
        {
            return 0;
        }

        // cube root function fitted between -27 and +27, scaled to (0, 1)
        
        var from = PrizesDrawn.LastOrDefault(Start);
        var to = from + (End - from) / (NumberOfPrizeDraws - PrizesDrawn.Count);
        var fraction = (dt - from) / (to - from);
        return Math.Max(0, Math.Min(1, (Math.Cbrt(-27 + fraction * 27 * 2) + 3) / 6d));
    }
}