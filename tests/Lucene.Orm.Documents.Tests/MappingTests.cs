using Xunit;

namespace Lucene.Orm.Documents.Tests
{
    public class MappingTests
    {


        [Fact]
        public void Can_map_flat_object_default_mapping()
        {
            var mapping = Mapping.CreateFor<User>();
            var user = new User { FirstName = "John", LastName = "Doe", Age = 30 };

            var document = user.ToDocument(mapping);

            Assert.Equal("John", document.GetField(nameof(User.FirstName)).GetStringValue());
            Assert.Equal("Doe", document.GetField(nameof(User.LastName)).GetStringValue());
            Assert.Equal(30, document.GetField(nameof(User.Age)).GetInt32Value());
        }

        [Fact]
        public void Can_map_complex_object_default_mapping()
        {
            var mapping = Mapping.CreateFor<UserEx>();
            var user = new UserEx
            {
                FirstName = "John",
                LastName = "Doe",
                Age = 30,
                Address = new Address
                {
                    City = "Tel-Aviv",
                    Street = "Sesame str."
                }
            };

            var document = user.ToDocument(mapping);

            Assert.Equal("John", document.GetField(nameof(User.FirstName)).GetStringValue());
            Assert.Equal("Doe", document.GetField(nameof(User.LastName)).GetStringValue());

            Assert.Equal("Tel-Aviv", document.GetField($"{nameof(UserEx.Address)}.{nameof(Address.City)}").GetStringValue());
            Assert.Equal("Sesame str.", document.GetField($"{nameof(UserEx.Address)}.{nameof(Address.Street)}").GetStringValue());

            Assert.Equal(30, document.GetField(nameof(User.Age)).GetInt32Value());
        }

        [Fact]
        public void Can_map_simple_and_object_collections()
        {
            var customer = new Customer
            {
                Name = "John Doe",
                ContactDetails = new ContactDetails
                {
                    Phones = new[] { "1234", "4567" }
                },
                ShippingAddresses = new[]
                {
                    new Address{ City = "Tel-Aviv" },
                    new Address{ City = "Haifa" }
                }
            };

            var mapping = Mapping.CreateFor<Customer>();
            var document = customer.ToDocument(mapping);

            Assert.Equal("John Doe", document.GetField(nameof(Customer.Name)).GetStringValue());
        }
    }
}
