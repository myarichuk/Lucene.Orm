using Lucene.Net.Documents;
using System;

namespace Lucene.Orm.Documents
{
    public static class DocumentExtensions
    {
        //TODO: introduce overload that caches the mapping by type of TObject (mapping has no state!)
        public static Document ToDocument<TObject>(this TObject @object)
            where TObject : class
        {
            var doc = new Document();

            //foreach(var fieldMapping in mapping.FieldMappings)
            //    foreach(var field in fieldMapping.Value(@object))
            //        doc.Add(field);

            return doc;
        }

        public static TObject ToObject<TObject>(this Document document)
            where TObject : class
        {            

            throw new NotImplementedException();
        }
    }
}
