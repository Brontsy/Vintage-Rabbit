using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Common.Entities;
using Vintage.Rabbit.Common.Serialization;
using Vintage.Rabbit.Interfaces.Membership;
using Vintage.Rabbit.Parties.Entities;

namespace Vintage.Rabbit.Parties.Repositories
{
    internal interface IPartyRepository
    {
        PagedResult<Party> GetParties(int page, int resultsPerPage);

        Party GetPartyByGuid(Guid guid);

        Party GetPartyByOrderGuid(Guid orderGuid);
        
        void SaveParty(Party party, IActionBy actionBy);
    }

    internal class PartyRepository : IPartyRepository
    {
        private ISerializer _serializer;
        private string _connectionString;

        public PartyRepository(ISerializer serializer)
        {
            this._connectionString = ConfigurationManager.ConnectionStrings["VintageRabbit"].ConnectionString;
            this._serializer = serializer;
        }

        public PagedResult<Party> GetParties(int page, int resultsPerPage)
        {
            PagedResult<Party> parties = new PagedResult<Party>();
            parties.PageNumber = page;
            parties.ItemsPerPage = resultsPerPage;

            string sql = @"Select VintageRabbit.Parties.* From VintageRabbit.Parties
                            Inner Join VintageRabbit.Orders On VintageRabbit.Parties.OrderGuid = VintageRabbit.Orders.Guid 
                            Where VintageRabbit.Orders.Status In ('Complete', 'AwaitingShipment')
                            Order By VintageRabbit.Parties.DateCreated Desc 
                            OFFSET @Offset ROWS FETCH NEXT @ResultsPerPage ROWS ONLY;
                            Select Count(*) From VintageRabbit.Parties;";

            int offset = (page - 1) * resultsPerPage;

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                using (var multi = connection.QueryMultiple(sql, new { Offset = offset, ResultsPerPage = resultsPerPage }))
                {
                    parties.AddRange(multi.Read<Party>());
                    parties.TotalResults = multi.Read<int>().First();
                }
            }

            return parties;
        }

        public Party GetPartyByGuid(Guid guid)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                var parties = connection.Query<Party>("Select * From VintageRabbit.Parties Where Guid = @Guid", new { Guid = guid });

                if (parties.Any())
                {
                    return parties.First();
                }
            }

            return null;
        }

        public Party GetPartyByOrderGuid(Guid orderGuid)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                var parties = connection.Query<Party>("Select * From VintageRabbit.Parties Where OrderGuid = @OrderGuid", new { OrderGuid = orderGuid });

                if (parties.Any())
                {
                    return parties.First();
                }
            }

            return null;
        }

        public void SaveParty(Party party, IActionBy actionBy)
        {
            if(this.GetPartyByGuid(party.Guid) == null)
            {
                string sql = @"Insert Into VintageRabbit.Parties (Guid, OrderGuid, Status, PartyDate, DropoffAddress, PickupAddress, ChildsName, Age, PartyTime, [PartyAddress], [RSVPDetails], DateCreated, DateLastModified, LastModifiedBy) 
                                Values (@Guid, @OrderGuid, @Status, @PartyDate, @DropoffAddress, @PickupAddress, @ChildsName, @Age, @PartyTime, @PartyAddress, @RSVPDetails, @DateCreated, @DateLastModified, @LastModifiedBy)";

                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Execute(sql, new
                    {
                        Guid = party.Guid,
                        OrderGuid = party.OrderGuid,
                        Status = party.Status.ToString(),
                        PartyDate = party.PartyDate,
                        DropoffAddress = party.DropoffAddress,
                        PickupAddress = party.PickupAddress,
                        ChildsName = party.ChildsName,
                        Age = party.Age,
                        PartyTime = party.PartyTime,
                        PartyAddress = party.PartyAddress,
                        RSVPDetails = party.RSVPDetails,
                        DateCreated = DateTime.Now,
                        DateLastModified = DateTime.Now,
                        LastModifiedBy = actionBy.Email
                    });
                }
            }
            else
            {
                string sql = @"Update VintageRabbit.Parties Set OrderGuid = @OrderGuid, Status = @Status, PartyDate = @PartyDate, DropoffAddress = @DropoffAddress, 
                                PickupAddress = @PickupAddress, ChildsName = @ChildsName, Age = @Age, PartyTime = @PartyTime, [PartyAddress] = @PartyAddress, RSVPDetails = @RSVPDetails, 
                                DateLastModified = @DateLastModified, LastModifiedBy = @LastModifiedBy Where Guid = @Guid";

                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Execute(sql, new
                    {
                        Guid = party.Guid,
                        OrderGuid = party.OrderGuid,
                        Status = party.Status.ToString(),
                        PartyDate = party.PartyDate,
                        DropoffAddress = party.DropoffAddress,
                        PickupAddress = party.PickupAddress,
                        ChildsName = party.ChildsName,
                        Age = party.Age,
                        PartyTime = party.PartyTime,
                        PartyAddress = party.PartyAddress,
                        RSVPDetails = party.RSVPDetails,
                        DateLastModified = DateTime.Now,
                        LastModifiedBy = actionBy.Email
                    });
                }
            }
        }

    }
}
