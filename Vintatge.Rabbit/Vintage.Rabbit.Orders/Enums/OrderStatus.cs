using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vintage.Rabbit.Orders.Enums
{
    public enum OrderStatus
    {
        Initialised,
        AwaitingHireDelivery,
        AwaitingHirePickup,
        AwaitingShipment,
        Complete
    }
}
