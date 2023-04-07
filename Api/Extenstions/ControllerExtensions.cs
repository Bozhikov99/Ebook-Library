using Common.ApiConstants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace Api.Extenstions
{
    public static class ControllerExtensions
    {
        public static string GetAbsoluteAction(this ControllerBase controller, string controllerName, string action, object values)
        {
            ArgumentNullException.ThrowIfNull(controller);

            HttpRequest request = controller.Request;
            string scheme = request.Scheme;
            string host = request
                .Host
                .Value;

            string link = controller
                .Url
                .Action(action, controllerName, values, scheme, host);

            return link;
        }

        public static string GetAbsoluteAction(this ControllerBase controller, string action, object values)
        {
            ArgumentNullException.ThrowIfNull(controller);

            string controllerFullName = controller
                .GetType()
                .Name;

            string controllerName = controllerFullName.Substring(0, controllerFullName.Length - 10);

            HttpRequest request = controller.Request;
            string scheme = request.Scheme;
            string host = request
                .Host
                .Value;

            string link = controller
                .Url
                .Action(action, controllerName, values, scheme, host);

            return link;
        }

        public static bool IsHateoasRequired(this ControllerBase controller)
        {
            bool isHateoasSet = controller
                .ControllerContext
                .HttpContext
                .Request
                .Headers
                .TryGetValue(HeaderConstants.HATEOAS, out StringValues header);

            if (isHateoasSet)
            {
                try
                {
                    bool isHateoasRequired = bool.Parse(header.ToString());

                    return isHateoasRequired;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }
    }
}
