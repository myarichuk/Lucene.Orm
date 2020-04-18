using Lucene.Net.Documents;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Lucene.Orm.Documents
{
    public class DocumentCreationVisitor
    {
        public Document Visit(IDictionary<string, MemberInfo> objectStructure, object source)
        {
            throw new NotImplementedException();
        }

    }
}
