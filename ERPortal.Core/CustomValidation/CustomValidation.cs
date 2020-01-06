using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.ModelBinding;
using System.Web.Mvc;
using ModelMetadata = System.Web.Mvc.ModelMetadata;

namespace ERPortal.Core.CustomValidation
{

    class CustomValidationClassAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly string _chars;
        public CustomValidationClassAttribute(string chars)
            : base("{0} contains invalid character.")
        {
            _chars = chars;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var valueAsString = value.ToString();
                for (int i = 0; i < _chars.Length; i++)
                {
                    if (valueAsString.Contains(_chars[i]))
                    {
                        var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                        return new ValidationResult(errorMessage);
                    }
                }
            }
            return ValidationResult.Success;
        }
        //new method  
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationParameters.Add("chars", _chars);
            rule.ValidationType = "customvalidationclass";
            yield return rule;
        }
    }
}



