using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day7
{
    public class Hand : IComparable<Hand>
    {
        public string HandString { get; }
        public int Bid { get; }

        private readonly int handType;

        public Hand(string hand, int bid)
        {
            this.HandString = hand;
            this.Bid = bid;

            handType = CalculateHandType(hand);

        }

        private static int CalculateHandType(string hand)
        {
            var nbEquals = hand.Select(c => hand.Count(d => c == d)).OrderDescending().ToList();

            return nbEquals[0] switch
            {
                5 => 6,// Five of kind
                4 => 5,// Four of kind
                3 when nbEquals[3] == 2 => 4,// Full house
                3 => 3, // Threee of kind
                2 when nbEquals[2] == 2 => 2, // Two Pair
                2 => 1, // Pair
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

            for (int i = 0; i < 5; i++)
            {
                var diff = CardToStrengh(HandString[i]) - CardToStrengh(other.HandString[i]);
                if (diff != 0)
                {
                    return diff;
                }
            }
            return 0;

        }

        private static int CardToStrengh(char card)
        {
            return card switch
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
}
