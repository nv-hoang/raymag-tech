using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using RefineCMS.Attributes;

namespace RefineCMS.Extensions;

public class AdminPrefixConvention(string _adminSlug) : IApplicationModelConvention
{
    public void Apply(ApplicationModel application)
    {
        foreach (var controller in application.Controllers)
        {
            // Only apply to controllers with [AdminController]
            var adminAttr = controller.Attributes
                .OfType<AdminControllerAttribute>()
                .FirstOrDefault();

            if (adminAttr == null)
                continue;

            string route = _adminSlug;
            if (!string.IsNullOrEmpty(adminAttr.Name))
            {
                route = $"{route}/{adminAttr.Name}";
            }

            // Clear existing routes and set exact match
            controller.Selectors.Clear();

            controller.Selectors.Add(new SelectorModel
            {
                AttributeRouteModel = new AttributeRouteModel(
                    new RouteAttribute(route)
                )
            });
        }
    }
}
