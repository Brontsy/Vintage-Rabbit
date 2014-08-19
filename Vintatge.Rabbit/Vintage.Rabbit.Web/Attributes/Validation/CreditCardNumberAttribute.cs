using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vintage.Rabbit.Web.Attributes.Validation
{
    public class CreditCardNumberAttribute : RegularExpressionAttribute
    {
        static CreditCardNumberAttribute()
        {
            // necessary to enable client side validation
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(CreditCardNumberAttribute), typeof(RegularExpressionAttributeAdapter));
        }

        public CreditCardNumberAttribute()
            // the following regex came from http://www.regular-expressions.info/creditcard.html
            : base(@"^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14}|3[47][0-9]{13})$")
        {
        }
    }
}