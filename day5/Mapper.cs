namespace day5
{
    public record Interval()
    {
        public long Start
        {
            get;
            set;
        }
        public long End
        {
            get;
            set;
        }
    };

    public class Mapper
    {
        private record Mapping(long Dest, long StartSource, long Length)
        {
            private readonly long diff = Dest - StartSource;
            public long EndSource { get; } = StartSource + Length;

            public bool IsMappable(long sourceValue) => sourceValue >= StartSource && sourceValue < EndSource;
            public long Map(long sourceValue)
            {
                return sourceValue + diff;
            }
        }

        public string Source { get; }
        public string Destination { get; }
        private readonly List<Mapping> mappings = [];
        public Mapper(string source, string target)
        {
            Source = source;
            Destination = target;
        }

        public void AddMapping(long dest, long source, long length)
        {
            mappings.Add(new Mapping(dest, source, length));
        }

        public long Map(long sourceValue)
        {
            var mapper = mappings.Find(x => x.IsMappable(sourceValue));
            return mapper is null ? sourceValue : mapper.Map(sourceValue);
        }

        public IEnumerable<Interval> Map(Interval interval)
        {
            return Split(interval).Select(splited => new Interval
            {
                Start = Map(splited.Start),
                End = Map(splited.End)
            });
        }


        private IEnumerable<Interval> Split(Interval interval)
        {

            //                  IS ............ IE
            //   M1S ... M1E
            //              M2S  ..M2E
            //                        M3S..M3E
            //                                 M4S.....M4E

            //                  IS ............ IE
            //              M5S.....................M5E

            foreach (var mapping in mappings.OrderBy(m => m.StartSource))
            {
                // M1
                if (interval.Start >= mapping.EndSource)
                {
                    continue;
                }
                // M5
                if (interval.Start >= mapping.StartSource && interval.End < mapping.EndSource)
                {
                    continue;
                }

                //M2
                if (interval.Start >= mapping.StartSource && interval.End >= mapping.EndSource)
                {
                    yield return new Interval { Start = interval.Start, End = mapping.EndSource - 1 };
                    interval.Start = mapping.EndSource;
                    continue;
                }
                // M3
                if (interval.Start < mapping.StartSource && interval.End >= mapping.EndSource)
                {
                    yield return new Interval() { Start = interval.Start, End = mapping.StartSource - 1 };
                    yield return new Interval() { Start = mapping.StartSource, End = mapping.EndSource - 1 };
                    interval.Start = mapping.EndSource;
                    continue;
                }
                //M4
                if (interval.Start < mapping.StartSource && interval.End >= mapping.StartSource)
                {
                    yield return new Interval { Start = interval.Start, End = mapping.StartSource - 1 };
                    interval.Start = mapping.StartSource;
                    continue;
                }
                if (interval.End < mapping.StartSource)
                {
                    // mapping are sorted 
                    break;
                }

            }
            yield return new Interval { Start = interval.Start, End = interval.End };
        }
    }
}
