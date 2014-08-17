using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Interfaces.Messaging;

namespace Vintage.Rabbit.Interfaces.Products
{
    public interface IProductImage
    {
        Guid Id { get; }

        string SecureUrl { get; }

        string SecureThumbnail { get; }
    }

}
