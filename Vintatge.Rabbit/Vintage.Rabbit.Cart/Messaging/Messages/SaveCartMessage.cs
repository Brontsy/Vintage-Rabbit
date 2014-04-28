using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Carts.Entities;
using Vintage.Rabbit.Interfaces.Messaging;

namespace Vintage.Rabbit.Carts.Messaging.Messages
{
    public class SaveCartMessage : IMessage
    {
        public Cart Cart { get; private set; }

        public SaveCartMessage(Cart cart)
        {
            this.Cart = cart;
        }
    }
}
