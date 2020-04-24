using Lucene.Net.Documents;
using Lucene.Orm.Documents.AST;
using Lucene.Orm.Documents.Extensions;
using System;

namespace Lucene.Orm.Documents
{
    public static class DocumentExtensions
    {
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

        public static Document ToDocument<TObject>(this TObject @object) where TObject : class
        {
            var fields = @object.ConvertToFields();
            var document = new Document();

            foreach(var field in fields)
                document.Add(field);

            return document;
        }

        public static TObject ToObject<TObject>(this Document document)
            where TObject : class
        {            

            throw new NotImplementedException();
        }
    }
}
