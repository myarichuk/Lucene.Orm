using System.Collections.Generic;

namespace Lucene.Orm.Documents.Tests
{
    //here, some members are fields and some properties, this is intentional
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
            
        public int Age; 
    }

    public class UserEx : User
    {
        public Address Address { get; set; }
    }

    public class Address
    {
        public string City;
        public string Street;
    }

    public class ContactDetails
    {
        public string[] Phones { get; set; }
        public List<string> Emails { get; set; }
    }

    public class Customer
    {
        public string Name;

        public ContactDetails ContactDetails;

        public Address[] ShippingAddresses { get; set; }
    }
}
