using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vintage.Rabbit.Web.Attributes.Validation
{
    public class CCVAttribute : ValidationAttribute, IClientValidatable
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var propertyInfo = validationContext.ObjectType.GetProperty("CardType");

            var propertyValue = propertyInfo.GetValue(validationContext.ObjectInstance, null);

            if (value == null)
            {
                return new ValidationResult("Please enter your CCV number");
            }

            if (value.ToString().Length != 4 && value.ToString().Length != 3)
            {
                return new ValidationResult("Your CCV number must be 3 or 4 digits long");
            }
         
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule ccvAttribute = new ModelClientValidationRule();
            ccvAttribute.ErrorMessage = this.ErrorMessage;
            ccvAttribute.ValidationType = "ccv";
            yield return ccvAttribute;

        }
    }
}