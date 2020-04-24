using Lucene.Net.Documents;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Fasterflect;

namespace Lucene.Orm.Documents
{   
    public class Mapping
    {
        internal readonly ConcurrentDictionary<string, Func<object, Field>> FieldMapping = new ConcurrentDictionary<string, Func<object, Field>>();        

        internal Mapping() { }

        public Type Type { get; private set;}

        public static Mapping CreateFor<TObject>()
        {
            var mapping = new Mapping { Type = typeof(TObject) };
            
            var structureMap = TypeStructureMapper.Instance.Map<TObject>();

            foreach(var kvp in structureMap)
            {
                _ = mapping.FieldMapping.TryAdd(kvp.Key, instance =>
                  {
                      var fieldValue = GetValueRecursive(kvp.Key.Split('.'), instance);
                      if(fieldValue == null)
                          return null;
                      return fieldValue.ToField(kvp.Key);
                  });
            }

            object GetValueRecursive(IEnumerable<string> fieldNameParts, object value)
            {
                var current = value;
                foreach (string namePart in fieldNameParts)
                {
                    current = current.TryGetValue(namePart);
                    if(current == null)
                        return null;
                }
                
                return current;
            }

            return mapping;
        }      
    }
}
