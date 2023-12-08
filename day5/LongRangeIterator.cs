using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day5
{
    public sealed class LongRange : IEnumerable<long>
    {
        private readonly long start;
        private readonly long length;

        public LongRange(long start, long length)
        {
            this.start = start;
            this.length = length;
        }

        public IEnumerator<long> GetEnumerator()
        {
            return new LongRangeIterator(start, length);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new LongRangeIterator(start, length);
        }
    }


    public sealed class LongRangeIterator : IEnumerator<long>
    {
        private readonly long _start;
        private readonly long _end;

        public LongRangeIterator(long start, long count)
        {
            _start = start;
            _end = start + count;
            Reset();
        }

        public long Current { get; private set; }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            Reset();
        }

        public bool MoveNext()
        {
            if (Current == _end)
            {
                return false;
            }
            Current++;
            return true;
        }

        public void Reset()
        {
            Current = _start - 1;
        }
    }
}
