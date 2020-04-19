using Lucene.Net.Documents;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections;

namespace Lucene.Orm.Documents
{   
    public class Mapping
    {
        internal readonly ConcurrentDictionary<string, Func<object, MemberInfo, Field>> FieldCreators = new ConcurrentDictionary<string, Func<object, MemberInfo, Field>>();        

        internal Mapping() { }

        public Type Type { get; private set;}

        public static Mapping CreateFor<TObject>()
        {
            var mapping = new Mapping { Type = typeof(TObject) };
            
            var structureMap = TypeStructureMapper.Instance.Map<TObject>();

            return mapping;
        }      
    }
}
