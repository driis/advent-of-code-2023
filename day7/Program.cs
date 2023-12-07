// Normally we will start by reading lines from an input file

using System.Numerics;

var input = File.ReadAllLines(args.FirstOrDefault() ?? "input.txt");
var hands = input.Select(Hand.Parse);

var handsOrdered = hands
    .Select(x => new ComplexHand(x.BestWithJoker(), x))
    .Order()
    .Select((h, i) => new {Hand = h, Rank = i + 1, Winnings = (i+1) * h.Original.Bid}).ToArray();

handsOrdered.DumpConsole();
int answer = handsOrdered.Sum(h => h.Winnings);
WriteLine($"Part 2:\nAnswer: {answer}");

record Hand(string Cards, int Bid) : IComparable<Hand>
{
    public static Hand Parse(string line)
    {
        var parts = line.Split(' ');
        return new Hand(parts[0], parts[1].ToInt());
    }

    public IEnumerable<IGrouping<char, char>> CardsOfKind => Cards.GroupBy(_ => _);
    public int HighestCountOfKind => CardsOfKind.Max(x => x.Count());

    public bool HasJoker => Cards.Any(ch => ch == 'J');
    public Hand BestWithJoker()
    {
        if (!HasJoker)
            return this;
        int jokerIndex = Cards.IndexOf('J');
        var jokerChars = _cardsOfValue[..^2];
        var handsToConsider = jokerChars.Select(ch => new Hand(new String(Cards.ReplaceAt(jokerIndex, ch)), Bid))
            .Select(h => h.BestWithJoker());
        
        return handsToConsider.OrderDescending().First();
    }
    public int CompareTo(Hand? other)
    {
        var handsCompared = CompareToHandOnly(other);
        if (handsCompared != 0)
            return handsCompared;
        
        // Same hand
        return CompareToCardsOnly(other);
    }

    public int CompareToCardsOnly(Hand? other)
    {
        for (int i = 0; i < Cards.Length; i++)
        {
            if (this.Cards[i] != other.Cards[i])
            {
                var indexThis = _cardsOfValue.IndexOf(this.Cards[i]);
                var indexOther = _cardsOfValue.IndexOf(other.Cards[i]);
                int cardsCompared = indexOther - indexThis;
                if (cardsCompared != 0)
                {
                    return cardsCompared;
                }
            }
        }
        
        return 0;
    }

    public int CompareToHandOnly(Hand? other)
    {
        Debug.Assert(!ReferenceEquals(null, other));
        var typeThis = HandType.WinningOrder.First(x => x.Predicate(this));
        var typeOther = HandType.WinningOrder.First(x => x.Predicate(other));
        int handsCompared = typeThis.Value - typeOther.Value;
        return handsCompared;
    }

    private readonly List<char> _cardsOfValue = new() {'A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2','J'};
}

record ComplexHand(Hand Best, Hand Original) : IComparable<ComplexHand>
{
    public int CompareTo(ComplexHand? other)
    {
        Debug.Assert(!ReferenceEquals(other,null));
        int n = Best.CompareToHandOnly(other.Best);
        if (n == 0)
        {
            n = Original.CompareToCardsOnly(other.Original);
        }

        return n;
    }

    public override string ToString()
    {
        return new {Best = Best.Cards, Original = Original.Cards, Original.Bid}.ToString() ?? throw new InvalidOperationException();
    }
}
record HandType(Func<Hand, bool> Predicate, int Value)
{
    public static HandType FiveOfAKind => new HandType(hand => hand.HighestCountOfKind == 5, 10);
    public static HandType FourOfAKind => new HandType(hand => hand.HighestCountOfKind == 4, 9);
    public static HandType FullHouse => new HandType(hand => hand.HighestCountOfKind == 3 && hand.CardsOfKind.Count() == 2, 8);
    public static HandType ThreeOfAKind => new HandType(hand => hand.HighestCountOfKind == 3 && hand.CardsOfKind.Count() == 3, 7);
    public static HandType TwoPair => new HandType(hand => hand.CardsOfKind.Count(x => x.Count() == 2) == 2, 6);
    public static HandType OnePair => new HandType(hand => hand.HighestCountOfKind == 2, 5);

    public static HandType None => new HandType(_ => true, 0);

    public static HandType[] WinningOrder { get; } = {FiveOfAKind, FourOfAKind, FullHouse, ThreeOfAKind, TwoPair, OnePair, None};
}