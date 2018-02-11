// Copyright (c) Team Agility. All rights reserved.

namespace Indspire.Soaring.Engagement.Models
{
    using System;
    using Microsoft.AspNetCore.Http;

    public class InstanceSelector : IInstanceSelector
    {
#pragma warning disable CC0021 // Use nameof
        private static string cookieName = "InstanceID";
#pragma warning restore CC0021 // Use nameof

        public InstanceSelector(IHttpContextAccessor httpContextAccessor)
        {
            this.HttpContext = httpContextAccessor == null
                ? throw new ArgumentNullException(nameof(httpContextAccessor))
                : httpContextAccessor.HttpContext;
        }

        public InstanceSelector(HttpContext httpContext)
        {
            this.HttpContext = httpContext ??
                throw new ArgumentNullException(nameof(httpContext));
        }

        public HttpContext HttpContext { get; private set; }

        public int InstanceID
        {
            get
            {
                var selectedInstanceCookieIDAsString =
                    this.HttpContext.Request.Cookies.ContainsKey(cookieName)
                        ? this.HttpContext.Request.Cookies[cookieName]
                        : string.Empty;

                var selectedInstanceID =
                    !string.IsNullOrWhiteSpace(selectedInstanceCookieIDAsString) &&
                    int.TryParse(selectedInstanceCookieIDAsString, out int tempInstanceID)
                            ? tempInstanceID
                            : -1;

                return selectedInstanceID;
            }

            set
            {
                this.HttpContext.Response.Cookies.Append(
                  cookieName,
                  value.ToString(),
                  new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Strict });
            }
        }
    }
}
