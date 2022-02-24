using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
//using System.Web.Mvc;

namespace Autenticazione.Helpers.Attributes
{
    public class BooleanRequiredTrueAttribute : ValidationAttribute //, IClientModelValidator
    {
        //public override bool IsValid(object value)
        //{
        //    return value != null && (bool)value == true;
        //}


        //public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
        //     ClientModelValidationContext context)
        //{
        //    yield return new ModelClientValidationRule(
        //        "cannotbered",
        //        FormatErrorMessage(ErrorMessage));
        //}
    }
}
