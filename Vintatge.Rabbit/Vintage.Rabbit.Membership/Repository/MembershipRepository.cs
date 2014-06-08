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
using Vintage.Rabbit.Membership.Enums;

namespace Vintage.Rabbit.Membership.Repository
{
    internal interface IMembershipRepository
    {
        Member GetMember(Guid memberId);

        Member GetMemberByEmail(string email);
    }

    internal class MembershipRepository : IMembershipRepository, IMessageHandler<SaveMemberMessage>
    {
        private ISerializer _serializer;
        private string _connectionString;

        public MembershipRepository(ISerializer serializer)
        {
            this._connectionString = ConfigurationManager.ConnectionStrings["VintageRabbit"].ConnectionString;
            this._serializer = serializer;
        }

        public Member GetMember(Guid memberId)
        {
            string sql = @"Select * From VintageRabbit.Members Where Id = @MemberId;
                           Select Role From VintageRabbit.MemberRoles Where MemberId = @MemberId";

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                using (var multi = connection.QueryMultiple(sql, new { MemberId = memberId }))
                {
                    Member member = multi.Read<Member>().Single();
                    IList<string> roles = multi.Read<string>().ToList();

                    member.Roles = roles.Select(o => (Role)Enum.Parse(typeof(Role), o)).ToList();

                    return member;
                }
            }
        }

        public Member GetMemberByEmail(string email)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                var members = connection.Query<Member>("Select * From VintageRabbit.Members Where Email = @Email", new { Email = email });

                if (members.Any())
                {
                    Member member = members.First();

                    var roles = connection.Query<string>("Select Role From VintageRabbit.MemberRoles Where MemberId = @MemberId", new { MemberId = member .Id });
                    member.Roles = roles.Select(o => (Role)Enum.Parse(typeof(Role), o)).ToList();

                    return member;
                }
            }

            return null;
        }

        public void Handle(SaveMemberMessage message)
        {
            Member member = message.Member;

            if(this.GetMember(member.Id) == null)
            {
                // insert
                string sql = "Insert Into VintageRabbit.Members (Id, Email, Password, FirstName, LastName, DateCreated, DateLastModified) Values (@MemberId, @Email, @Password, @FirstName, @LastName, @DateCreated, @DateLastModified)";

                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Execute(sql, new
                    {
                        MemberId = member.Id,
                        Email = member.Email,
                        Password = member.Password,
                        FirstName = member.FirstName,
                        LastName = member.LastName,
                        DateCreated = DateTime.Now,
                        DateLastModified = DateTime.Now
                     
                    });
                }
            }
            else
            {
                //update
                string sql = @"Updated VintageRabbit.Members Set Email = @Email, Password = @Password, FirstName = @FirstName, LastName = @LastName, DateLastModified = @DateLastModified Where Id = @MemberId";

                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Query(sql, new
                    {
                        MemberId = member.Id,
                        Email = member.Email,
                        Password = member.Password,
                        FirstName = member.FirstName,
                        LastName = member.LastName,
                        DateLastModified = DateTime.Now

                    });
                }
            }
        }
    }
}
