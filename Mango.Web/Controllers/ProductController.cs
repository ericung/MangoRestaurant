using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto> list = new List<ProductDto>();
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await productService.GetAllProductsAsync<ResponseDto>(accessToken);
            if (response!= null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        public IActionResult ProductCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductCreate(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var response = await productService.CreateProductAsync<ResponseDto>(productDto, accessToken);
                if (response!= null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }

            return View(productDto);
        }

        public async Task<IActionResult> ProductEdit(int productId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await productService.GetProductByIdAsync<ResponseDto>(productId, accessToken);
            if (response!= null && response.IsSuccess)
            {
                ProductDto model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                return View(model);
            }

            return NotFound();
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductEdit(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var response = await productService.UpdateProductAsync<ResponseDto>(productDto, accessToken);
                if (response!= null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }

            return View(productDto);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ProductDelete(int productId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await productService.GetProductByIdAsync<ResponseDto>(productId, accessToken);
            if (response!= null && response.IsSuccess)
            {
                ProductDto model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                return View(model);
            }

            return NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductDelete(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var response = await productService.DeleteProductAsync<ResponseDto>(model.ProductId, accessToken);
                if ((response is object)&&(response.IsSuccess))
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }

            return View(model);
        }
    }
}
