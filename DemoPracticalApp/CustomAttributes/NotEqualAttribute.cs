using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DemoPracticalApp.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class NotEqualAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly string _currentPasswordPropertyName;

        public NotEqualAttribute(string currentPasswordPropertyName)
        {
            _currentPasswordPropertyName = currentPasswordPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var currentPasswordProperty = validationContext.ObjectType.GetProperty(_currentPasswordPropertyName);

            if (currentPasswordProperty == null)
            {
                throw new ArgumentException("Property with this name not found", _currentPasswordPropertyName);
            }

            var currentPasswordValue = currentPasswordProperty.GetValue(validationContext.ObjectInstance);
            var newPasswordValue = value;

            if (currentPasswordValue != null && currentPasswordValue.Equals(newPasswordValue))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-differentpasswords", ErrorMessage);

            var currentPasswordProperty = context.ModelMetadata.ContainerType.GetProperty(_currentPasswordPropertyName);
            if (currentPasswordProperty != null)
            {
                var currentPasswordDisplayName = currentPasswordProperty.GetCustomAttribute<DisplayAttribute>()?.Name;
                MergeAttribute(context.Attributes, "data-val-differentpasswords-currentpassword", currentPasswordDisplayName);
            }
        }

        private bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if (attributes.ContainsKey(key))
            {
                return false;
            }

            attributes.Add(key, value);
            return true;
        }
    }

}
