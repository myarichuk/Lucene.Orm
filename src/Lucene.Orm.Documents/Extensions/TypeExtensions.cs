using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Lucene.Orm.Documents.AST
{
    internal static class TypeExtensions
    {
        private readonly static Type TypeOfString = typeof(string);
        private readonly static Type TypeOfIEnumerable = typeof(IEnumerable<>);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFieldCandidate(this Type type) =>
            type.IsValueType || TypeOfString.IsAssignableFrom(type);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsTraversable(this Type type) =>
            type.IsClass && !TypeOfString.IsAssignableFrom(type);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsCollection(this Type type) =>
            TypeOfIEnumerable.IsAssignableFrom(type);


    }
}
