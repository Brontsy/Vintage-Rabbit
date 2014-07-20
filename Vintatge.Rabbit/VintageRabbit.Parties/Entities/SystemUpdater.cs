using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Membership;

namespace Vintage.Rabbit.Parties.Entities
{
    public class SystemUpdater : IActionBy
    {
        public string Email
        {
            get { return "System"; }
        }
    }
}
