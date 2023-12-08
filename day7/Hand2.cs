using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day7
{
    public class Hand2 : IComparable<Hand2>
    {
        public string HandString { get; }
        public int Bid { get; }

        private readonly int handType;

        public Hand2(string hand, int bid)
        {
            this.HandString = hand;
            this.Bid = bid;

            handType = CalculateHandType(hand);

        }

        private static int CalculateHandType(string hand)
        {
            var nbEquals = hand.Select(c => hand.Count(d => c == d && c != 'J')).OrderDescending().ToList();
            var nbJoker = hand.Count(c => c == 'J');



            return nbEquals[0] switch
            {
                5 => 6,// Five of kind
                4 when nbJoker == 1 => 6,
                3 when nbJoker == 2 => 6,
                2 when nbJoker == 3 => 6,
                1 when nbJoker == 4 => 6,
                0 when nbJoker == 5 => 6,
                4 => 5,// Four of kind
                3 when nbJoker == 1 => 5,
                2 when nbJoker == 2 => 5,
                1 when nbJoker == 3 => 5,
                3 when nbEquals[3] == 2 => 4,// Full house
                2 when nbEquals[2] == 2 && nbJoker == 1 => 4,
                2 when nbEquals[2] == 1 && nbJoker == 2 => 4,
                3 => 3, // Threee of kind
                2 when nbJoker == 1 => 3,
                1 when nbJoker == 2 => 3,
                2 when nbEquals[2] == 2 => 2, // Two Pair
                2 => 1, // Pair
                1 when nbJoker == 1 => 1,
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
                'J' => 1,
                'T' => 10,
                _ => card - '0',
            };

        }
    }
}
