using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Messaging;
using Dapper;
using System.Configuration;
using Vintage.Rabbit.Inventory.Entities;
using Vintage.Rabbit.Inventory.Enums;
using Vintage.Rabbit.Common.Serialization;

namespace Vintage.Rabbit.Inventory.Repository
{
    internal interface IInventoryRepository
    {
        IList<InventoryItem> GetInventoryByProductGuid(Guid productGuid);

        InventoryItem GetInventoryByGuid(Guid guid);

        InventoryItem SaveInventory(InventoryItem inventory);
    }

    internal class InventoryRepository : IInventoryRepository
    {
        private ISerializer _serializer;
        private string _connectionString;

        public InventoryRepository(ISerializer serializer)
        {
            this._connectionString = ConfigurationManager.ConnectionStrings["VintageRabbit"].ConnectionString;
            this._serializer = serializer;
        }

        public InventoryItem GetInventoryByGuid(Guid guid)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                var inventoryResults = connection.Query<dynamic>("Select * From VintageRabbit.Inventory Where Guid = @Guid", new { Guid = guid });

                if (inventoryResults.Any())
                {
                    return this.ConvertToInventory(inventoryResults.First());
                }
            }

            return null;
        }

        public IList<InventoryItem> GetInventoryByProductGuid(Guid productGuid)
        {
            IList<InventoryItem> inventoryItems = new List<InventoryItem>();

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                var inventoryResults = connection.Query<dynamic>("Select * From VintageRabbit.Inventory Where ProductGuid = @ProductGuid Order By DateCreated Desc", new { ProductGuid = productGuid });

                foreach (var inventory in inventoryResults)
                {
                    InventoryItem item = this.ConvertToInventory(inventory);

                    inventoryItems.Add(item);
                }
            }

            return inventoryItems;
        }

        private InventoryItem ConvertToInventory(dynamic inventory)
        {
            IList<DateTime> datesUnavailable = this._serializer.Deserialize<List<DateTime>>(inventory.DatesUnavailable);

            InventoryItem inventoryItem = new InventoryItem()
            {
                Id = inventory.Id,
                Guid = inventory.Guid,
                ProductGuid = inventory.ProductGuid,
                Status = (InventoryStatus)Enum.Parse(typeof(InventoryStatus), inventory.Status),
                DatesUnavailable = datesUnavailable,
                DateCreated = inventory.DateCreated
            };

            return inventoryItem;
        }



        public InventoryItem SaveInventory(InventoryItem inventory)
        {
            if (this.GetInventoryByGuid(inventory.Guid) == null)
            {
                // insert
                string sql = "Insert Into VintageRabbit.Inventory (Guid, ProductGuid, Status, DatesUnavailable, DateCreated, DateLastModified) Values (@Guid, @ProductGuid, @Status, @DatesUnavailable, @DateCreated, @DateLastModified)";

                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Execute(sql, new
                    {
                        Guid = inventory.Guid,
                        ProductGuid = inventory.ProductGuid,
                        Status = inventory.Status.ToString(),
                        DatesUnavailable = this._serializer.Serialize(inventory.DatesUnavailable),
                        DateCreated = DateTime.Now,
                        DateLastModified = DateTime.Now

                    });
                }
            }
            else
            {
                //update
                string sql = @"Update VintageRabbit.Inventory Set ProductGuid = @ProductGuid, Status = @Status, OrderItemGuid = @OrderItemGuid, DateSold = @DateSold, DatesUnavailable = @DatesUnavailable, DateLastModified = @DateLastModified Where Guid = @Guid";

                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Execute(sql, new
                    {
                        Guid = inventory.Guid,
                        ProductGuid = inventory.ProductGuid,
                        Status = inventory.Status.ToString(),
                        DatesUnavailable = this._serializer.Serialize(inventory.DatesUnavailable),
                        DateCreated = DateTime.Now,
                        DateLastModified = DateTime.Now,
                        OrderItemGuid = inventory.OrderItemGuid,
                        DateSold = inventory.DateSold
                    });
                }
            }

            return inventory;
        }
    }
}
