using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vintage.Rabbit.Products.Entities
{
    public class Category
    {
        public int Id { get; internal set; }

        public string Name { get; internal set; }

        public string DisplayName { get; internal set; }
    }
}
