using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Fasterflect;

namespace Lucene.Orm.Documents
{
    public class ObjectStructureVisitor
    {
        private static readonly ConcurrentDictionary<Type, IReadOnlyDictionary<string, MemberInfo>> _parseCache = new ConcurrentDictionary<Type, IReadOnlyDictionary<string, MemberInfo>>();      

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyDictionary<string, MemberInfo> Visit<TObject>() =>
            Visit(typeof(TObject));

        public IReadOnlyDictionary<string, MemberInfo> Visit(Type currentType)
        {
            if(_parseCache.TryGetValue(currentType, out var cachedValue))
                return cachedValue;

            var result = new Dictionary<string, MemberInfo>();
            var currentMemberPath = new Stack<string>();

            var fields = currentType.FieldsAndProperties(Flags.InstancePublic);

            for(int i = 0; i < fields.Count; i++)
                Visit(fields[i], result, currentMemberPath);

            _parseCache.TryAdd(currentType, result);
            return result;
        }

        private void Visit(MemberInfo memberInfo, Dictionary<string, MemberInfo> result, Stack<string> currentMemberPath)
        {
            var currentType = memberInfo.Type();

            if(currentType == typeof(string) || 
                !currentType.IsClass ||
                typeof(IEnumerable).IsAssignableFrom(currentType))
                    result.Add(string.Join(".", currentMemberPath.Reverse().Concat(memberInfo.Name)), memberInfo);

            var fields = currentType.FieldsAndProperties(Flags.InstancePublic);
            for(int i = 0; i < fields.Count; i++)
            {
                var childType = fields[i].Type();

                if(childType.IsValueType)
                    continue;

                currentMemberPath.Push(memberInfo.Name);
                Visit(fields[i], result, currentMemberPath);
                currentMemberPath.Pop();
            }
        }
    }
}
