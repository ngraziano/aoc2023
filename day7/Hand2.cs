namespace day7
{
    public class Hand2 : IComparable<Hand2>
    {
        public string HandString { get; }
        public int Bid { get; }

        private readonly int handType;

        public Hand2(string hand, int bid)
        {
            HandString = hand;
            Bid = bid;
            handType = CalculateHandType(hand);
        }

        private static int CalculateHandType(string hand)
        {
            var nbEquals = hand.Select(c => hand.Count(d => c == d && c != 'J')).OrderDescending().ToList();
            var nbJoker = hand.Count(c => c == 'J');

            return (nbEquals, nbJoker) switch
            {
                ([5, ..], 0) => 6,// Five of kind
                ([4, ..], 1) => 6,
                ([3, ..], 2) => 6,
                ([2, ..], 3) => 6,
                ([1, ..], 4) => 6,
                ([0, ..], 5) => 6,
                ([4, ..], 0) => 5,// Four of kind
                ([3, ..], 1) => 5,
                ([2, ..], 2) => 5,
                ([1, ..], 3) => 5,
                ([3, _, _, 2, ..], 0) => 4,// Full house
                ([2, _, 2, ..], 1) => 4,
                ([3, ..], 0) => 3, // Threee of kind
                ([2, ..], 1) => 3,
                ([1, ..], 2) => 3,
                ([2, _, 2, ..], 0) => 2, // Two Pair
                ([2, ..], 0) => 1, // Pair
                ([1, ..], 1) => 1,
                _ => 0,
            };
        }

        public int CompareTo(Hand2? other)
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
            'J' => 1,
            'T' => 10,
            _ => card - '0',
        };
    }
}
