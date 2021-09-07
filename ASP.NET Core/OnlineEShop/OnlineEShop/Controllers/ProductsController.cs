namespace OnlineEShop.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OnlineEShop.Services.Products;
    using OnlineEShop.ViewModels;
    using System;
    using System.Threading.Tasks;

    public class ProductsController : Controller
    {
        private readonly IProductsService productsService;
        public ProductsController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        [Authorize]
        public IActionResult Add()
        {
            var viewModel = new CreateProductInputModel();
            viewModel.CategoriesItems = this.productsService.GetAllAsKeyValuePairs();
            return this.View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(CreateProductInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.CategoriesItems = this.productsService.GetAllAsKeyValuePairs();
                return this.View(input);
            }

            try
            {
                await this.productsService.CreateAsync(input);
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                input.CategoriesItems = this.productsService.GetAllAsKeyValuePairs();
                return this.View(input);
            }

            return this.Redirect("/");
        }

        public IActionResult AllByCategory(int id)
        {
            var viewModel = new ProductsViewModels
            {
                Products = this.productsService.AllProductsInCategory(id)
            };

            return this.View(viewModel);
        }

        public IActionResult AllById(int id)
        {
            var product = this.productsService.ById(id);

            return this.View(product);
        }
    }
}
