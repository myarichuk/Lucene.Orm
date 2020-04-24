using System;
using static Lucene.Net.Documents.Field;

namespace Lucene.Orm.Documents.Schema
{
    [AttributeUsage(AttributeTargets.Field)]
    public class LuceneFieldAttribute : Attribute
    {
        public Store Store { get; set; } = Constants.DefaultStore;
    }
}
