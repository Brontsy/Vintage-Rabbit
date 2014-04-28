using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vintage.Rabbit.Interfaces.Data
{
    public interface IEntity
    {
        Guid Id { get; }
    }
}
