using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Indspire.Soaring.Engagement.Models
{
    public class InstanceSelector
    {
        public static string cookieName = "InstanceID";

        public HttpContext HttpContext { get; private set; }

        public InstanceSelector(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            this.HttpContext = httpContext;
        }

        public int GetInstanceID()
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

        public void SetInstanceID(int instanceID)
        {
            HttpContext.Response.Cookies.Append(
                cookieName,

                instanceID.ToString(),

                new CookieOptions()
                {
                    HttpOnly = true,

                    SameSite = SameSiteMode.Strict
                });
        }
    }
}
