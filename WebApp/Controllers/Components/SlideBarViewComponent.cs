using Admin_APP.Services.Categories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Controllers.Components
{
    public class SlideBarViewComponent : ViewComponent
    {
        private readonly ICategoriesApiClient _categoriesApiClient;

        public SlideBarViewComponent(ICategoriesApiClient categoriesApiClient)
        {
            _categoriesApiClient = categoriesApiClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _categoriesApiClient.GetAll(CultureInfo.CurrentCulture.Name);
            return View(items);
        }
    }
}