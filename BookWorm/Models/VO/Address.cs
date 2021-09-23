using System;

namespace BookWorm.Models.VO
{
    public class Address
    {
        private int _id;
        private string _localAddress;
        private int _apartment = 0;
        private string _city;
        private string _postalCode;
        private Country _country;
        private Region _region;

        public Address() { }

        public Address(string localAddress, int apartment, string city, string postalCode, Country country, Region region)
        {
            LocalAddress = localAddress;
            Apartment = apartment;
            City = city;
            PostalCode = postalCode;
            Country = country;
            Region = region;
        }

        public int Id { get => _id; set => _id = value; }
        public string LocalAddress { get => _localAddress; set => _localAddress = value; }
        public int Apartment { get => _apartment; set => _apartment = value; }
        public string City { get => _city; set => _city = value; }
        public string PostalCode { get => _postalCode; set => _postalCode = value; }
        public Country Country { get => _country; set => _country = value; }
        public Region Region { get => _region; set => _region = value; }

        public string CountryName
        {
            get => Country.Name;
            set
            {
                if (Country == null)
                    Country = new Country();

                Country.Name = value;
            }
        }

        public string RegionName
        {
            get => Region.Name;
            set
            {
                if (Region == null)
                    Region = new Region();

                Region.Name = value;
            }
        }

        public override string ToString()
        {
            return String.Format("{0}, {1}, {2}, {3}, {4}, {5}",
               LocalAddress, Apartment, City, Region.Name, PostalCode, Country.Name);
        }
    }
}
