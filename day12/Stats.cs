using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day12
{
    struct  Stats
    {
        public int knowFailed;
        public int knowPassed;
        public int unknown;
        public int totalFailed;
        public int totalPassed;

        


        public static Stats CalculateStats(Span<char> condition, IEnumerable<int> conditionGroups)
        {
            return new Stats()
            {
                knowFailed = condition.Count('#'),
                knowPassed = condition.Count('.'),
                unknown = condition.Count('?'),
                totalFailed = conditionGroups.Sum(),
                totalPassed = condition.Length - conditionGroups.Sum(),

            };
        }

        public bool CheckStat()
        {
            return
                knowFailed + unknown >= totalFailed &&
                knowPassed + unknown >= totalPassed;

        }

        public Stats DecPassed()
        {
            return new Stats()
            {
                knowFailed = knowFailed,
                knowPassed = knowPassed +1,
                unknown = unknown - 1,
                totalFailed = totalFailed,
                totalPassed = totalPassed,

            };
        }

        public Stats DecFailed()
        {
            return new Stats()
            {
                knowFailed = knowFailed + 1,
                knowPassed = knowPassed,
                unknown = unknown - 1,
                totalFailed = totalFailed,
                totalPassed = totalPassed,

            };
        }
    };

}
