using Lucene.Net.Documents;
using Lucene.Orm.Documents.AST;
using Lucene.Orm.Documents.Extensions;
using System;
using System.Runtime.Serialization;
using Utf8Json;
using Microsoft.IO;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Lucene.Orm.Documents
{
    public static class DocumentExtensions
    {
        private readonly static RecyclableMemoryStreamManager MemoryStreamManager = new RecyclableMemoryStreamManager();

        //TODO: add handling of LucenFieldAttribute
        //TODO: make sure LuceneFieldAttribute has all necessary stuff for FieldType
        public static Field ToField<TObject>(this TObject @object, string name)
        {
            switch (Type.GetTypeCode(@object.GetType()))
            {
                case TypeCode.Boolean:
                    return new StringField(name, Convert.ToBoolean(@object) ? "true" : "false" ,Constants.DefaultStore);
                case TypeCode.Char:
                    return new StringField(name, Convert.ToChar(@object).ToString(), Constants.DefaultStore);
                case TypeCode.DateTime:
                    return new StringField(name, DateTools.DateToString(Convert.ToDateTime(@object), DateTools.Resolution.MILLISECOND), Constants.DefaultStore);
                case TypeCode.DBNull:
                    return new StringField(name, "dbnull" ,Constants.DefaultStore);
                case TypeCode.Single:
                case TypeCode.Double:
                    return new DoubleField(name, Convert.ToDouble(@object), Constants.DefaultStore);
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.SByte:
                    return new Int32Field(name, Convert.ToInt32(@object), Constants.DefaultStore);
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                    return new Int64Field(name, Convert.ToInt64(@object), Constants.DefaultStore);
                case TypeCode.UInt64:
                    return new Field(name, 
                        BitConverter.GetBytes(Convert.ToUInt64(@object)), 
                            new FieldType{ IsIndexed = true, NumericType = NumericType.INT64, IsStored = Constants.DefaultStore == Field.Store.YES });
                case TypeCode.Decimal:
                    return new Field(name, 
                        BitConverterEx.GetBytes(Convert.ToDecimal(@object)), 
                            new FieldType{ IsIndexed = true, NumericType = NumericType.DOUBLE, IsStored = Constants.DefaultStore == Field.Store.YES });
                case TypeCode.String:
                    return new StringField(name, Convert.ToString(@object), Constants.DefaultStore);
                default:
                    return null;
            }
        }

        public static Document ToDocument<TObject>(this TObject @object) 
            where TObject : class
        {
            using(var tempStream = MemoryStreamManager.GetStream())
            {
                JsonSerializer.Serialize(tempStream, @object);
                var dynamicObject = (IDictionary<string, object>)JsonSerializer.Deserialize<dynamic>(tempStream);

                var document = new Document();
                foreach(var field in ParseFields(dynamicObject, new Stack<string>()))
                    document.Add(field);

                return document;

                IEnumerable<Field> ParseFields(IDictionary<string, object> obj, Stack<string> path)
                {
                    foreach(var kvp in obj)
                    {
                        string name = string.Join(".", path.Reverse().Concat(kvp.Key));
                        switch(kvp.Value)
                        {
                            case string str:
                                if(str != null)
                                    yield return str.ToField(name);
                                break;
                            case IDictionary<string,object> dict:
                                path.Push(name);
                                foreach(var field in ParseFields(dict, path))
                                    yield return field;
                                path.Pop();
                                break;
                            case IEnumerable enumerable:
                                foreach(var item in enumerable)
                                    if(item is IDictionary<string, object> subDict)
                                    {
                                        path.Push(name);
                                        foreach(var field in ParseFields(subDict, path))
                                            yield return field;
                                        path.Pop();
                                    }
                                    else 
                                    {
                                        if(item != null)
                                            yield return item.ToField(name);
                                    }
                                break;
                            default:
                                if(kvp.Value != null)
                                    yield return kvp.Value.ToField(name);
                                break;
                        }
                    }
                }
            }
        }

        public static TObject ToObject<TObject>(this Document document)
            where TObject : class
        {            
            var @object = FormatterServices.GetUninitializedObject(typeof(TObject));
      
            

            throw new NotImplementedException();
        }
    }
}
