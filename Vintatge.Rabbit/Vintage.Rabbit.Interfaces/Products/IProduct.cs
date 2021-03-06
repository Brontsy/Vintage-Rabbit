﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Interfaces.Messaging;

namespace Vintage.Rabbit.Interfaces.Products
{
    public interface IPurchaseable
    {
        Guid Guid { get; }

        ProductType Type { get; }

        string Title { get; }

        decimal Cost { get; }

        int Inventory { get; }
    }

    public interface IProduct : IMessage
    {
        int Id { get; }

        Guid Guid { get; }

        ProductType Type { get; }

        string Title { get; }

        /// <summary>
        /// Number of inventory the product has
        /// </summary>
        int Inventory { get; }

        decimal Cost { get; }

        IList<IProductImage> Images { get; }

    }
}
