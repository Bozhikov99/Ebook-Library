using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;

namespace Web.ModelBinders
{
    public class DateTimeModelBinder : IModelBinder
    {
        private readonly string dateFormat;

        public DateTimeModelBinder(string dateFormat)
        {
            this.dateFormat = dateFormat;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            ValueProviderResult valueResult = bindingContext
                .ValueProvider
                .GetValue(bindingContext.ModelName);

            if (valueResult != ValueProviderResult.None && !string.IsNullOrWhiteSpace(valueResult.FirstValue))
            {
                DateTime actualValue = DateTime.MinValue;
                string dateValue = valueResult.FirstValue;

                bool isSuccessful = false;

                try
                {
                    actualValue = DateTime.ParseExact(dateValue, dateFormat, CultureInfo.InvariantCulture);

                    isSuccessful = true;
                }
                catch (FormatException)
                {
                    try
                    {
                        actualValue = DateTime.Parse(dateValue, new CultureInfo("bg-bg"));
                        isSuccessful = true;
                    }
                    catch (Exception ex)
                    {

                        bindingContext.ModelState
                            .AddModelError(bindingContext.ModelName, ex, bindingContext.ModelMetadata);
                    }


                }
                catch (Exception ex)
                {
                    bindingContext.ModelState
                       .AddModelError(bindingContext.ModelName, ex, bindingContext.ModelMetadata);
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
