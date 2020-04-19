using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Lucene.Orm.Documents.Tests
{
    public class TypeStructureMapperTests
    {
        [Fact]
        public void Simple_object_should_be_properly_parsed()
        {
            var structureMap = TypeStructureMapper.Instance.Map<User>();

            Assert.Equal(3, structureMap.Count);

            Assert.True(structureMap.ContainsKey(nameof(User.Age)));
            Assert.True(structureMap.ContainsKey(nameof(User.FirstName)));
            Assert.True(structureMap.ContainsKey(nameof(User.LastName)));
        }

        [Fact]
        public void Complex_object_should_be_properly_parsed()
        {
            var structureMap = TypeStructureMapper.Instance.Map<UserEx>();

            Assert.Equal(5, structureMap.Count);

            Assert.True(structureMap.ContainsKey(nameof(UserEx.Age)));
            Assert.True(structureMap.ContainsKey(nameof(UserEx.FirstName)));
            Assert.True(structureMap.ContainsKey(nameof(UserEx.LastName)));

            Assert.True(structureMap.ContainsKey($"{nameof(UserEx.Address)}.{nameof(UserEx.Address.City)}"));
            Assert.True(structureMap.ContainsKey($"{nameof(UserEx.Address)}.{nameof(UserEx.Address.Street)}"));
        }

        [Fact]
        public void Objects_with_collections_should_be_properly_parsed()
        {
            var structureMap = TypeStructureMapper.Instance.Map<Customer>();            

            Assert.Equal(4, structureMap.Count);

            Assert.True(structureMap.ContainsKey($"{nameof(Customer.Name)}"));
            Assert.True(structureMap.ContainsKey($"{nameof(Customer.ShippingAddresses)}"));
            Assert.True(structureMap.ContainsKey($"{nameof(Customer.ContactDetails)}.{nameof(Customer.ContactDetails.Phones)}"));
            Assert.True(structureMap.ContainsKey($"{nameof(Customer.ContactDetails)}.{nameof(Customer.ContactDetails.Emails)}"));
        }
    }
}
