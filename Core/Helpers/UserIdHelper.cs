using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Core.Helpers
{
    public class UserIdHelper
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserIdHelper(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId()
        {
            string userId = httpContextAccessor.HttpContext
                .User
                .FindFirstValue(ClaimTypes.NameIdentifier);

            return userId;
        }
    }
}

