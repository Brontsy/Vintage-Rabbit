using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vintage.Rabbit.Payment.Entities.Eway
{
    public class AccessCodeResponse
    {
        public string AccessCode { get; set; }

        public string FormActionUrl { get; set; }

        public Payment Payment { get; set; }
    }
}
