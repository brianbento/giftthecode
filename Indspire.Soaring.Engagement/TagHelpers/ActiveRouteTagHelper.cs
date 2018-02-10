// Copyright (c) Team Agility. All rights reserved.

namespace Indspire.Soaring.Engagement.TagHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [HtmlTargetElement(Attributes = "is-active-route")]
    public class ActiveRouteTagHelper : TagHelper
    {
        private IDictionary<string, string> routeValues;

        /// <summary>The name of the action method.</summary>
        /// <remarks>Must be <c>null</c> if <see cref="P:Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper.Route" /> is non-<c>null</c>.</remarks>
        [HtmlAttributeName("asp-action")]
        public string Action { get; set; }

        /// <summary>The name of the controller.</summary>
        /// <remarks>Must be <c>null</c> if <see cref="P:Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper.Route" /> is non-<c>null</c>.</remarks>
        [HtmlAttributeName("asp-controller")]
        public string Controller { get; set; }

        /// <summary>Additional parameters for the route.</summary>
        [HtmlAttributeName("asp-all-route-data", DictionaryAttributePrefix = "asp-route-")]
        public IDictionary<string, string> RouteValues
        {
            get
            {
                if (this.routeValues == null)
                {
                    this.routeValues = new Dictionary<string, string>(
                        StringComparer.OrdinalIgnoreCase);
                }

                return this.routeValues;
            }

            set
            {
                this.routeValues = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="T:Microsoft.AspNetCore.Mvc.Rendering.ViewContext" /> for the current request.
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            if (this.ShouldBeActive())
            {
                MakeActive(output);
            }

            output.Attributes.RemoveAll("is-active-route");
        }

        private bool ShouldBeActive()
        {
            var currentController = this.ViewContext.RouteData.Values["Controller"].ToString();
            var currentAction = this.ViewContext.RouteData.Values["Action"].ToString();

            if (!string.IsNullOrWhiteSpace(this.Controller) &&
                this.Controller.ToLower() != currentController.ToLower())
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(this.Action) &&
                this.Action.ToLower() != currentAction.ToLower())
            {
                return false;
            }

            foreach (KeyValuePair<string, string> routeValue in this.RouteValues)
            {
                if (!this.ViewContext.RouteData.Values.ContainsKey(routeValue.Key) ||
                    this.ViewContext.RouteData.Values[routeValue.Key].ToString() != routeValue.Value)
                {
                    return false;
                }
            }

            return true;
        }

        private static void MakeActive(TagHelperOutput output)
        {
            var classAttr = output.Attributes.FirstOrDefault(a => a.Name == "class");

            if (classAttr == null)
            {
                classAttr = new TagHelperAttribute("class", "active");
                output.Attributes.Add(classAttr);
            }
            else if (classAttr.Value == null || classAttr.Value.ToString().IndexOf("active") < 0)
            {
                output.Attributes.SetAttribute(
                    "class",
                    classAttr.Value == null ? "active" : classAttr.Value.ToString() + " active");
            }
        }
    }
}
