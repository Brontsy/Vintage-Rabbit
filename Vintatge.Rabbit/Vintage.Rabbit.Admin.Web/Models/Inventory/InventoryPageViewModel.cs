using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Admin.Web.Models.Products;
using Vintage.Rabbit.Interfaces.Inventory;
using Vintage.Rabbit.Inventory.Entities;
using Vintage.Rabbit.Products.Entities;

namespace Vintage.Rabbit.Admin.Web.Models.Inventory
{
    public class InventoryPageViewModel
    {
        public IList<InventoryItemViewModel> Inventory { get; private set; }

        public ProductViewModel Product { get; private set; }

        public AddInventoryViewModel AddInventory { get; private set; }

        public InventoryPageViewModel(Product product, IList<InventoryItem> inventory)
        {
            this.Product = new ProductViewModel(product, new List<Category>());
            this.Inventory = inventory.Select(o => new InventoryItemViewModel(o)).ToList();
            this.AddInventory = new AddInventoryViewModel();
        }
    }
}