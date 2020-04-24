using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Lucene.Orm.Documents.Tests
{
    public class DocumentConversionTests
    {
        public class User
        {
            public string Name { get; set; }
            public int Age;
        }

        [Fact]
        public void Flat_object_can_be_converted()
        {
            var user = new User { Name = "John Doe", Age = 30 };
            var doc = user.ToDocument();

            Assert.Equal(2, doc.Fields.Count);
            Assert.Equal("John Doe", doc.GetField(nameof(User.Name)).GetStringValue());
            Assert.Equal(30, doc.GetField(nameof(User.Age)).GetInt32Value());
        }

    }
}
