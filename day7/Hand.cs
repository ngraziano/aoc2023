namespace day7
{
    public class Hand : IComparable<Hand>
    {
        public string HandString { get; }
        public int Bid { get; }

        private readonly int handType;

        public Hand(string hand, int bid)
        {
            HandString = hand;
            Bid = bid;

            handType = CalculateHandType(hand);

        }

        private static int CalculateHandType(string hand)
        {
            var nbEquals = hand.Select(c => hand.Count(d => c == d)).OrderDescending().ToList();


            return nbEquals switch
            {
                [5, ..] => 6,// Five of kind
                [4, ..] => 5,// Four of kind
                [3, _, _, 2, _] => 4,// Full house
                [3, ..] => 3, // Threee of kind
                [2, _, 2, ..] when nbEquals[2] == 2 => 2, // Two Pair
                [2, ..] => 1, // Pair
                _ => 0,
            };

        }

        public int CompareTo(Hand? other)
        {
            if (other == null) return 1;
            var handiff = handType - other.handType;
            if (handiff != 0)
            {
                return handiff;
            }

            return HandString.Select(CardToStrengh)
                .Zip(other.HandString.Select(CardToStrengh))
                .Select(comp => comp.First - comp.Second)
                .FirstOrDefault(d => d != 0, 0);

        }

        private static int CardToStrengh(char card) => card switch
        {
            'A' => 14,
            'K' => 13,
            'Q' => 12,
            'J' => 11,
            'T' => 10,
            _ => card - '0',
        };
    }
}
