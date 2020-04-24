namespace Lucene.Orm.Documents.AST
{
    using Fasterflect;
    using Lucene.Net.Documents;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    internal static class Converter
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Field> ConvertToFields<TDocument>(this TDocument document) where TDocument : class
            => ConvertToFieldsInternal(document, new Stack<string>());

        private static IEnumerable<Field> ConvertToFieldsInternal(object document, Stack<string> path)
        {
            bool hasRetrievedFields = false;
            if (path.Count > 0)
            {
                var memberName = string.Join(".", path.Reverse());
                var memberType = document.GetType();

                if (memberType.IsFieldCandidate())
                {
                    yield return document.ToField(memberName);
                    hasRetrievedFields = true;
                }
                else if (document is IEnumerable enumerable)
                {
                    foreach (var item in enumerable)
                    {
                        if (item == null)
                            continue;
                        foreach (var field in ConvertMemberToFields(item.GetType(), memberName, item, path))
                            yield return field;
                    }
                    hasRetrievedFields = true;
                }
            }

            if (hasRetrievedFields)
                yield break;

            var fieldsAndProperties = document.GetType().FieldsAndProperties(Flags.InstancePublicDeclaredOnly);
            for (int i = 0; i < fieldsAndProperties.Count; i++)
            {
                path.Push(fieldsAndProperties[i].Name);
                var memberInstance = document.TryGetValue(fieldsAndProperties[i].Name);
                if (memberInstance == null)
                    continue;

                var memberName = string.Join(".", path.Reverse());
                var memberType = memberInstance.GetType();

                foreach (var field in ConvertMemberToFields(memberType, memberName, memberInstance, path))
                    yield return field;

                path.Pop();
            }
        }

        private static IEnumerable<Field> ConvertMemberToFields(Type memberType, string memberName, object memberInstance, Stack<string> path)
        {
            if (memberInstance == null)
                yield break;
            if (memberType.IsFieldCandidate())
                yield return memberInstance.ToField(memberName);
            else if (memberType.IsTraversable())
                foreach (var field in ConvertToFieldsInternal(memberInstance, path))
                    yield return field;
            else if (memberInstance is IEnumerable enumerable)
            {
                var isTraversableType = false;
                var checkedTraversable = false;
                foreach (var item in enumerable)
                {
                    if (item == null)
                        continue;
                    if (!checkedTraversable)
                    {
                        var itemType = item.GetType();
                        isTraversableType = itemType.IsTraversable();
                        checkedTraversable = true;
                    }

                    if (isTraversableType)
                    {
                        foreach (var field in ConvertToFieldsInternal(item, path))
                            yield return field;
                    }
                    else yield return item.ToField(memberName);
                }
            }
        }
    }
}
