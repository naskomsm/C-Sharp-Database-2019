﻿namespace FastFood.Web.Controllers
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;

    using Data;
    using Models;
    using FastFood.Web.ViewModels.Categories;

    public class CategoriesController : Controller
    {
        private readonly FastFoodContext context;
        private readonly IMapper mapper;

        public CategoriesController(FastFoodContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateCategoryInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Home");
            }

            var category = this.mapper.Map<Category>(model);

            this.context.Categories.Add(category);

            this.context.SaveChanges();

            return this.RedirectToAction("All", "Categories");
        }

        public IActionResult All()
        {
            var categories = this.context
                .Categories
                .ProjectTo<CategoryAllViewModel>(mapper.ConfigurationProvider)
                .ToList();

            return this.View(categories);
        }
    }
}
