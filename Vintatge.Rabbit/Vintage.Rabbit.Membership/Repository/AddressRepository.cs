using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Interfaces.Serialization;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Membership.Messaging.Messages;

namespace Vintage.Rabbit.Membership.Repository
{
    internal interface IAddressRepository
    {
        Address GetAddress(Guid addressId);

        Address SaveAddress(Address address);
    }

    internal class AddressRepository : IAddressRepository
    {
        private ISerializer _serializer;
        private string _connectionString;

        public AddressRepository(ISerializer serializer)
        {
            this._connectionString = ConfigurationManager.ConnectionStrings["VintageRabbit"].ConnectionString;
            this._serializer = serializer;
        }

        public Address GetAddress(Guid addressGuid)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                var addresses = connection.Query<Address>("Select * From VintageRabbit.Addresses Where Guid = @Guid", new { Guid = addressGuid });

                if (addresses.Any())
                {
                    return addresses.First();
                }
            }

            return null;
        }
        
        public Address SaveAddress(Address address)
        {
            if (this.GetAddress(address.Guid) == null)
            {
                // insert
                string sql = @"Insert Into VintageRabbit.Addresses (Guid, MemberGuid, FirstName, LastName, CompanyName, Line1, SuburbCity, Postcode, State, IsShippingAddress, DateCreated, DateLastModified) Values 
                            (@Guid, @MemberGuid, @FirstName, @LastName, @CompanyName, @Line1, @SuburbCity, @Postcode, @State, @IsShippingAddress, @DateCreated, @DateLastModified)";

                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Execute(sql, new
                    {
                        Guid = address.Guid,
                        MemberGuid = address.MemberGuid,
                        FirstName = address.FirstName,
                        LastName = address.LastName,
                        CompanyName = address.CompanyName,
                        Line1 = address.Line1,
                        SuburbCity = address.SuburbCity,
                        Postcode = address.Postcode,
                        State = address.State.ToString(),
                        IsShippingAddress = address.IsShippingAddress,
                        DateCreated = DateTime.Now,
                        DateLastModified = DateTime.Now

                    });
                }
            }
            else
            {
                //update
                string sql = @"Updated VintageRabbit.Addresses Set 
                                MemberGuid = @MemberGuid, FirstName = @FirstName, LastName = @LastName,  CompanyName = @CompanyName, Line1 = @Line1, SuburbCity = @SuburbCity, 
                                Postcode = @Postcode, State = @State, IsShippingAddress = @IsShippingAddress, DateLastModified = @DateLastModified Where Guid = @Guid";

                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Execute(sql, new
                    {
                        Guid = address.Guid,
                        MemberGuid = address.MemberGuid,
                        FirstName = address.FirstName,
                        LastName = address.LastName,
                        CompanyName = address.CompanyName,
                        Line1 = address.Line1,
                        SuburbCity = address.SuburbCity,
                        Postcode = address.Postcode,
                        State = address.State.ToString(),
                        IsShippingAddress = address.IsShippingAddress,
                        DateLastModified = DateTime.Now
                    });
                }
            }

            return address;
        }
    }
}
