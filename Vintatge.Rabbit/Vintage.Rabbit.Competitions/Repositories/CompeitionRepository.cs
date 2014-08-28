using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Common.Serialization;
using Vintage.Rabbit.Competitions.Entities;
using Vintage.Rabbit.Interfaces.Membership;

namespace Vintage.Rabbit.Competitions.Repositories
{
    internal interface ICompeitionRepository
    {
        void SaveCompeitionEntry(CompetitionEntry entry);
    }

    internal class CompeitionRepository : ICompeitionRepository
    {
        private ISerializer _serializer;
        private string _connectionString;

        public CompeitionRepository(ISerializer serializer)
        {
            this._connectionString = ConfigurationManager.ConnectionStrings["VintageRabbit"].ConnectionString;
            this._serializer = serializer;
        }

        public void SaveCompeitionEntry(CompetitionEntry entry)
        {
            string sql = @"Insert Into VintageRabbit.CompetitionEntries (Guid, CompetitionName, Name, DateOfBirth, Email, PhoneNumber, EntryText, DateCreated) 
                                Values (@Guid, @CompetitionName, @Name, @DateOfBirth, @Email, @PhoneNumber, @EntryText, @DateCreated)";

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Execute(sql, new
                {
                    Guid = entry.Guid,
                    CompetitionName = entry.CompetitionName,
                    Name = entry.Name,
                    DateOfBirth = entry.DateOfBirth,
                    Email = entry.Email,
                    PhoneNumber = entry.PhoneNumber,
                    EntryText = entry.EntryText,
                    DateCreated = DateTime.Now
                });
            }
        }

    }
}
