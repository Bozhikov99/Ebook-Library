using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;

namespace Web.ModelBinders
{
    public class DecimalModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            ValueProviderResult valueResult = bindingContext
                .ValueProvider
                .GetValue(bindingContext.ModelName);

            if (valueResult != ValueProviderResult.None && !string.IsNullOrWhiteSpace(valueResult.FirstValue))
            {
                decimal actualValue = 0;
                bool isSuccessful = false;

                try
                {
                    string decimalValue = valueResult.FirstValue;
                    decimalValue = decimalValue.Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                    decimalValue = decimalValue.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

                    actualValue = Convert.ToDecimal(decimalValue);
                    isSuccessful = true;
                }
                catch (FormatException fe)
                {
                    bindingContext.ModelState
                        .AddModelError(bindingContext.ModelName, fe, bindingContext.ModelMetadata);
                }

                if (isSuccessful)
                {
                    bindingContext.Result = ModelBindingResult.Success(actualValue);
                }
            }

            return Task.CompletedTask;
        }
    }
}
