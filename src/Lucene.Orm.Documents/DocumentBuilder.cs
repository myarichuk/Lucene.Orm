using Lucene.Net.Documents;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Fasterflect;
using System.Reflection;
using System.Linq;
using System.Collections;

namespace Lucene.Orm.Documents.AST
{

    internal static class Converter
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Field> ConvertToFields<TDocument>(this TDocument document) where TDocument : class
            => ConvertToFieldsInternal(document, new Stack<string>(), new HashSet<string>());

        private static IEnumerable<Field> ConvertToFieldsInternal(object document, Stack<string> path, HashSet<string> alreadyVisited)
        {           
            var fieldsAndProperties = document.GetType().FieldsAndProperties(Flags.InstancePublicDeclaredOnly);
            for (int i = 0; i < fieldsAndProperties.Count; i++)
            {
                path.Push(fieldsAndProperties[i].Name);                
                var memberInstance = document.TryGetValue(fieldsAndProperties[i].Name);
                if(memberInstance == null)
                    continue;

                var memberName = string.Join(".", path.Reverse());

                var memberType = memberInstance.GetType();
                if(memberType.IsFieldCandidate())
                    yield return memberInstance.ToField(memberName);
                else if(memberType.IsTraversable())
                    foreach(var field in ConvertToFieldsInternal(memberInstance, path, alreadyVisited))
                        yield return field;
                else if(memberType.IsCollection())
                {
                    var isTraversableType = false;
                    var checkedTraversable = false;
                    var enumerable = (IEnumerable)memberInstance;
                    foreach(var item in enumerable)
                    {
                        if(!checkedTraversable)
                        {
                            var itemType = item.GetType();
                            isTraversableType = itemType.IsTraversable();
                            checkedTraversable = true;
                        }

                        if(isTraversableType)
                        {
                            foreach(var field in ConvertToFieldsInternal(item, path, alreadyVisited))
                                yield return field;
                        }
                        else yield return item.ToField(memberName);
                    }
                }

                path.Pop();
            }
        }
    }
}
