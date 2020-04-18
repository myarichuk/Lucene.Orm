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
        private readonly static ObjectStructureVisitor ObjectStructureVisitor;

        internal Mapping() { }

        public Type Type { get; private set;}

        public static Mapping CreateFor<TObject>()
        {
            var mapping = new Mapping { Type = typeof(TObject) };
            
            var structureInfo = ObjectStructureVisitor.Visit<TObject>();

            return mapping;
        }

        //private static Field MapValueToField((IEnumerable<MemberInfo> Parents, MemberInfo Current) memberInfo, string fieldName, Field field, object value, IReadOnlyDictionary<Type, Func<object, Field>> fieldMappings)
        //{
        //    switch (value)
        //    {
        //        case string str when str.Length <= 64:
        //            field = new StringField(fieldName, str, Field.Store.NO);
        //            break;
        //        case char c:
        //            field = new StringField(fieldName, c.ToString(), Field.Store.NO);
        //            break;
        //        case bool b:                    
        //            field = new StringField(fieldName, b.ToString(), Field.Store.NO);
        //            break;
        //        case string str:
        //            field = new TextField(fieldName, str, Field.Store.NO);                    
        //            break;
        //        case byte @byte:
        //            field = new Int32Field(fieldName, @byte, Field.Store.NO);
        //            break;
        //        case int @int:
        //            field = new Int32Field(fieldName, @int, Field.Store.NO);
        //            break;
        //        case long @long:
        //            field = new Int64Field(fieldName, @long, Field.Store.NO);
        //            break;
        //        case float @float:
        //            field = new SingleField(fieldName, @float, Field.Store.NO);
        //            break;
        //        case double @double:
        //            field = new DoubleField(fieldName, @double, Field.Store.NO);
        //            break;
        //        case object @object when value.GetType().IsClass && !value.GetType().IsAbstract:
        //            //nothing to do since we we travel over the fields recursively
        //            break;
        //        case object @object when value.GetType().IsAbstract:
        //            throw new NotSupportedException("Abstract fields or properties are not supported");
        //        default:
        //            var type = value.GetType();
        //            if(fieldMappings.TryGetValue(type,out var mapFunc))
        //                field = mapFunc(value);
        //            else
        //                throw new NotSupportedException($"Fields of type {type.Name} are not supported");
        //            break;
        //    }

        //    return field;
        //}

        //private object TryGetValue(object instance, string fieldName)
        //{
        //    var fieldParts = fieldName.Split('.');
        //    var current = instance;
            
        //    for (int i = 0; i < fieldParts.Length; i++)
        //    {
        //        string part = (string)fieldParts[i];
        //        var value = current.TryGetFieldValue(part) ?? instance.TryGetPropertyValue(part);
        //        if(value == null && i < fieldParts.Length - 1)
        //            return null;
        //        if(i < fieldParts.Length - 1)
        //            current = value;
        //        else
        //            return value;
        //    }

        //    return null;
        //}
    }
}
