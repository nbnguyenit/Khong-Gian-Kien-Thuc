﻿using KnowledgeSpace.WebPortal.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KnowledgeSpace.WebPortal.Controllers.Components
{
    public class NavBarViewComponent : ViewComponent
    {
        private readonly ICategoryApiClient _categoryApiClient;
        public NavBarViewComponent(ICategoryApiClient categoryApiClient)
        {
            _categoryApiClient = categoryApiClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _categoryApiClient.GetCategories();
            return View("Default", categories);
        }
    }
}
