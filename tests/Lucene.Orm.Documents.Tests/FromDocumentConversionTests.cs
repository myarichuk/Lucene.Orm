using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Lucene.Orm.Documents.Tests
{
    public class FromDocumentConversionTests
    {
        public class User
        {
            public string Name { get; set; }
            public int Age;
        }

        public class Address
        {
            public PhysicalAddress PhysicalAddress { get; set; }
            public string[] Emails { get; set; }
        }

        public class PhysicalAddress
        {
            public string City;
            public string Street;
        }

        public class UserExtended
        {
            public string Name;
            public int Age;
            public PhysicalAddress[] Addresses { get; set; }
        }

        public class UserExtendedWithList
        {
            public string Name;
            public int Age;
            public List<PhysicalAddress> Addresses { get; set; }
        }

        [Fact]
        public void Can_convert_to_from_simple_object()
        {
            var userEx = new UserExtended
            {
                Name = "John Doe",
                Age = 30,
                Addresses = new []
                {
                    new PhysicalAddress
                    {
                        City = "Tel-Aviv",
                        Street = "Sesame Str."
                    },
                    new PhysicalAddress
                    {
                        City = "Haifa",
                        Street = "White Str."
                    }
                }
            };   
            
            var doc = userEx.ToDocument();
            var convertedUser = doc.ToObject<UserExtended>();
        }
    }
}
