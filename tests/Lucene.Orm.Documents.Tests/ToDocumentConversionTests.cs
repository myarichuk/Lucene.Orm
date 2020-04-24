using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Lucene.Orm.Documents.Tests
{
    public class ToDocumentConversionTests
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
        public void Flat_object_can_be_converted()
        {
            var user = new User { Name = "John Doe", Age = 30 };
            var doc = user.ToDocument();

            Assert.Equal(2, doc.Fields.Count);
            Assert.Equal("John Doe", doc.GetField(nameof(User.Name)).GetStringValue());
            Assert.Equal(30, doc.GetField(nameof(User.Age)).GetInt32Value());
        }

        [Fact]
        public void Embedded_object_with_simple_arrays_can_be_converted()
        {
            var address = new Address
            {
                Emails = new [] { "foo@bar.com", "fobar@abc.com" },
                PhysicalAddress = new PhysicalAddress
                {
                    City = "Tel-Aviv",
                    Street = "Sesame Str."
                }
            };

            var doc = address.ToDocument();

            Assert.Equal(4, doc.Fields.Count);
            Assert.Equal("Tel-Aviv", doc.GetField($"{nameof(Address.PhysicalAddress)}.{nameof(Address.PhysicalAddress.City)}").GetStringValue());
            Assert.Equal("Sesame Str.", doc.GetField($"{nameof(Address.PhysicalAddress)}.{nameof(Address.PhysicalAddress.Street)}").GetStringValue());
            Assert.Equal(address.Emails, doc.GetValues(nameof(Address.Emails)));
        }

        [Fact]
        public void Embedded_object_with_object_arrays_can_be_converted()
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
            Assert.Equal(6, doc.Fields.Count);
            Assert.Equal("John Doe", doc.GetField(nameof(User.Name)).GetStringValue());
            Assert.Equal(30, doc.GetField(nameof(User.Age)).GetInt32Value());

            Assert.Equal(userEx.Addresses.Select(a => a.City).ToArray(), 
                doc.GetValues($"{nameof(UserExtended.Addresses)}.{nameof(Address.PhysicalAddress.City)}"));

            Assert.Equal(userEx.Addresses.Select(a => a.Street).ToArray(), 
                doc.GetValues($"{nameof(UserExtended.Addresses)}.{nameof(Address.PhysicalAddress.Street)}"));
        }

        [Fact]
        public void Embedded_object_with_object_generic_list_can_be_converted()
        {
            var userEx = new UserExtendedWithList
            {
                Name = "John Doe",
                Age = 30,
                Addresses = new List<PhysicalAddress>
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
            Assert.Equal(6, doc.Fields.Count);
            Assert.Equal("John Doe", doc.GetField(nameof(User.Name)).GetStringValue());
            Assert.Equal(30, doc.GetField(nameof(User.Age)).GetInt32Value());

            Assert.Equal(userEx.Addresses.Select(a => a.City).ToArray(), 
                doc.GetValues($"{nameof(UserExtended.Addresses)}.{nameof(Address.PhysicalAddress.City)}"));

            Assert.Equal(userEx.Addresses.Select(a => a.Street).ToArray(), 
                doc.GetValues($"{nameof(UserExtended.Addresses)}.{nameof(Address.PhysicalAddress.Street)}"));
        }

        [Fact]
        public void Embedded_object_with_object_generic_list_with_nulls_can_be_converted()
        {
            var userEx = new UserExtendedWithList
            {
                Name = "John Doe",
                Age = 30,
                Addresses = new List<PhysicalAddress>
                {
                    new PhysicalAddress
                    {
                        City = "Tel-Aviv",
                        Street = "Sesame Str."
                    },
                    null,
                    new PhysicalAddress
                    {
                        City = "Haifa",
                        Street = "White Str."
                    }
                }
            };

            var doc = userEx.ToDocument();
            Assert.Equal(6, doc.Fields.Count);
            Assert.Equal("John Doe", doc.GetField(nameof(User.Name)).GetStringValue());
            Assert.Equal(30, doc.GetField(nameof(User.Age)).GetInt32Value());

            Assert.Equal(userEx.Addresses.Where(x => x != null).Select(a => a.City).ToArray(), 
                doc.GetValues($"{nameof(UserExtended.Addresses)}.{nameof(Address.PhysicalAddress.City)}"));

            Assert.Equal(userEx.Addresses.Where(x => x != null).Select(a => a.Street).ToArray(), 
                doc.GetValues($"{nameof(UserExtended.Addresses)}.{nameof(Address.PhysicalAddress.Street)}"));
        }
    }
}
