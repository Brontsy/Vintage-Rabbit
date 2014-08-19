using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vintage.Rabbit.Web.Attributes.Validation
{
    /// <summary>
    /// Makes sure that the expiry date is valid (greater than today)
    /// </summary>
    public class ValidExpiryAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary>
        /// constructs a new valid expiry attribute
        /// </summary>
        /// <param name="monthProperty">the name of property that contains the value for the month</param>
        /// <param name="yearProperty">the name of the property that contains the value for the year</param>
        /// <param name="required"></param>
        public ValidExpiryAttribute(string monthProperty, string yearProperty)
        {
            this.MonthProperty = monthProperty;
            this.YearProperty = yearProperty;
        }

        public string MonthProperty { get; set; }

        public string YearProperty { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var monthInfo = validationContext.ObjectType.GetProperty(this.MonthProperty);
            var yearInfo = validationContext.ObjectType.GetProperty(this.YearProperty);

            if (monthInfo.PropertyType == typeof(Nullable<int>) && yearInfo.PropertyType == typeof(Nullable<int>))
            {
                int? month = monthInfo.GetValue(validationContext.ObjectInstance, null) as Nullable<int>;
                int? year = yearInfo.GetValue(validationContext.ObjectInstance, null) as Nullable<int>;

                if (month.HasValue && year.HasValue)
                {
                    DateTime regoExpiry = new DateTime(year.Value, month.Value, 1);
                    DateTime compareDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                    if (regoExpiry < compareDate)
                    {
                        return new ValidationResult(ErrorMessageString);
                    }
                }
            }

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule validExpiry = new ModelClientValidationRule();
            validExpiry.ErrorMessage = this.ErrorMessage;
            validExpiry.ValidationType = "validexpiry";
            validExpiry.ValidationParameters.Add("monthproperty", this.MonthProperty);
            validExpiry.ValidationParameters.Add("yearproperty", this.YearProperty);
            yield return validExpiry;

        }
    }
}