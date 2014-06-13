using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Membership.Messaging.Messages;
using Vintage.Rabbit.Membership.Enums;
using Vintage.Rabbit.Common.Serialization;

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

        public Member GetMember(Guid memberGuid)
        {
            string sql = @"Select * From VintageRabbit.Members Where Guid = @MemberGuid;
                           Select Role From VintageRabbit.MemberRoles Where MemberGuid = @MemberGuid";

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                using (var multi = connection.QueryMultiple(sql, new { MemberGuid = memberGuid }))
                {
                    IEnumerable<Member> members = multi.Read<Member>();

                    if (members.Any())
                    {
                        Member member = members.First();

                        IList<string> roles = multi.Read<string>().ToList();

                        member.Roles = roles.Select(o => (Role)Enum.Parse(typeof(Role), o)).ToList();

                        return member;
                    }

                    return null;
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

                    var roles = connection.Query<string>("Select Role From VintageRabbit.MemberRoles Where MemberGuid = @MemberGuid", new { MemberGuid = member.Guid });
                    member.Roles = roles.Select(o => (Role)Enum.Parse(typeof(Role), o)).ToList();

                    return member;
                }
            }

            return null;
        }

        public void Handle(SaveMemberMessage message)
        {
            Member member = message.Member;

            if(this.GetMember(member.Guid) == null)
            {
                // insert
                string sql = "Insert Into VintageRabbit.Members (Guid, Email, Password, FirstName, LastName, DateCreated, DateLastModified) Values (@Guid, @Email, @Password, @FirstName, @LastName, @DateCreated, @DateLastModified)";

                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Execute(sql, new
                    {
                        Guid = member.Guid,
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
                string sql = @"Updated VintageRabbit.Members Set Email = @Email, Password = @Password, FirstName = @FirstName, LastName = @LastName, DateLastModified = @DateLastModified Where Guid = @Guid";

                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Execute(sql, new
                    {
                        Guid = member.Guid,
                        Email = member.Email,
                        Password = member.Password,
                        FirstName = member.FirstName,
                        LastName = member.LastName,
                        DateLastModified = DateTime.Now

                    });
                }
            }

            // TODO: Look to make this a command!
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Execute("Delete From VintageRabbit.MemberRoles Where MemberGuid = @MemberGuid", new { MemberGuid = member.Guid });

                foreach(var role in member.Roles)
                {
                    connection.Execute("Insert Into VintageRabbit.MemberRoles (MemberGuid, [Role]) Values (@MemberGuid, @Role)", new { MemberGuid = member.Guid, Role = role.ToString() });
                }
            }
        }
    }
}
