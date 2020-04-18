using Lucene.Net.Documents;
using System;
using static Lucene.Net.Documents.Field;

namespace Lucene.Orm.Documents.Schema
{
    [AttributeUsage(AttributeTargets.Field)]
    public class LuceneField : Attribute
    {
        public Store Store { get; set; } = Constants.DefaultStore;
    }
}
