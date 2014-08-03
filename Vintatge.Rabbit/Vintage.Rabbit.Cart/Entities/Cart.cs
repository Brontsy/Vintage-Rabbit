using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Data;
using Vintage.Rabbit.Inventory.Entities;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Themes.Entities;

namespace Vintage.Rabbit.Carts.Entities
{
    public class Cart
    {
        public Guid Id { get; private set; }

        public Guid MemberId { get; private set; }

        public List<CartItem> Items { get; private set; }

        public decimal Total
        {
            get { return this.Items.Sum(o => o.Total); }
        }

        public Cart()
        {
            this.Id = Guid.NewGuid();
            this.Items = new List<CartItem>();
        }

        public Cart(Guid memberId) : this()
        {
            this.MemberId = memberId;
        }

        internal void AddProduct(int quantity, Product product, IList<InventoryItem> inventory)
        {
            int availableInventory = inventory.Count(o => o.IsAvailable());

            if (this.Items.Any(o => o.Product.Guid == product.Guid))
            {
                CartItem cartItem = this.Items.FirstOrDefault(o => o.Product.Guid == product.Guid);

                quantity += cartItem.Quantity;

                if(quantity > availableInventory)
                {
                    quantity = availableInventory;
                }

                cartItem.ChangeQuantity(quantity);
            }
            else
            {
                if (quantity > availableInventory)
                {
                    quantity = availableInventory;
                }

                this.Items.Add(new CartItem(quantity, product));
            }
        }

        internal void AddProduct(int quantity, Product product, DateTime partyDate, IList<InventoryItem> inventory)
        {
            int availableInventory = inventory.Count(o => o.IsAvailable(partyDate));

            if (this.Items.Any(o => o.Product.Guid == product.Guid))
            {
                CartItem cartItem = this.Items.FirstOrDefault(o => o.Product.Guid == product.Guid);

                quantity += cartItem.Quantity;

                if (quantity > availableInventory)
                {
                    quantity = availableInventory;
                }

                cartItem.ChangeQuantity(quantity);

                cartItem.Properties["PartyDate"] = partyDate;
            }
            else
            {
                if (quantity > availableInventory)
                {
                    quantity = availableInventory;
                }

                this.Items.Add(new HireCartItem(quantity, product, partyDate));
            }
        }

        internal void AddTheme(Theme theme, DateTime partyDate)
        {
            if (!this.Items.Any(o => o.Product.Guid == theme.Guid))
            {
                this.Items.Add(new CartItem(theme, partyDate));
            }
        }

        internal void RemoveProduct(Guid cartItemId)
        {
            CartItem cartItem = this.Items.FirstOrDefault(o => o.Id == cartItemId);

            if (cartItem != null)
            {
                this.Items.Remove(cartItem);
            }
        }

        internal void Clear()
        {
            this.Items = new List<CartItem>();
        }

        internal void ChangeMemberGuid(Guid memberGuid)
        {
            this.MemberId = memberGuid;
        }

        internal void UpdateQuantity(Guid cartItemId, int quantity)
        {
            CartItem item = this.Items.FirstOrDefault(o => o.Id == cartItemId);
            if(item != null)
            {
                item.ChangeQuantity(quantity);
            }
        }
    }
}
