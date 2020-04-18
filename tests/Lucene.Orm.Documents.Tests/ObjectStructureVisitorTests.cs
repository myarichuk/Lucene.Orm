using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Lucene.Orm.Documents.Tests
{
    public class ObjectStructureVisitorTests
    {
        private readonly ObjectStructureVisitor _objectStructureVisitor = new ObjectStructureVisitor();

        [Fact]
        public void Simple_object_should_be_properly_parsed()
        {
            var structureInfo = _objectStructureVisitor.Visit<User>();

            Assert.Equal(3, structureInfo.Count);

            Assert.True(structureInfo.ContainsKey(nameof(User.Age)));
            Assert.True(structureInfo.ContainsKey(nameof(User.FirstName)));
            Assert.True(structureInfo.ContainsKey(nameof(User.LastName)));
        }

        [Fact]
        public void Complex_object_should_be_properly_parsed()
        {
            var structureInfo = _objectStructureVisitor.Visit<UserEx>();

            Assert.Equal(5, structureInfo.Count);

            Assert.True(structureInfo.ContainsKey(nameof(UserEx.Age)));
            Assert.True(structureInfo.ContainsKey(nameof(UserEx.FirstName)));
            Assert.True(structureInfo.ContainsKey(nameof(UserEx.LastName)));

            Assert.True(structureInfo.ContainsKey($"{nameof(UserEx.Address)}.{nameof(UserEx.Address.City)}"));
            Assert.True(structureInfo.ContainsKey($"{nameof(UserEx.Address)}.{nameof(UserEx.Address.Street)}"));
        }

        [Fact]
        public void Objects_with_collections_should_be_properly_parsed()
        {
            var structureInfo = _objectStructureVisitor.Visit<Customer>();

            Assert.Equal(3, structureInfo.Count);

            Assert.True(structureInfo.ContainsKey($"{nameof(Customer.Name)}"));
            Assert.True(structureInfo.ContainsKey($"{nameof(Customer.ShippingAddresses)}"));
            Assert.True(structureInfo.ContainsKey($"{nameof(Customer.ContactDetails)}.{nameof(Customer.ContactDetails.Phones)}"));
        }
    }
}
