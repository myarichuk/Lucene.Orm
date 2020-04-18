using System.Collections.Generic;

namespace Lucene.Orm.Documents
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> enumerable, T additional)
        {
            foreach(var item in enumerable)
                yield return item;

            yield return additional;
        }
    }
}
