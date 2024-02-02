using AzureSqlWebApp.Models;
using AzureSqlWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AzureSqlWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IProductService _productService;
        public List<Product> Products { get; set; }
        public IndexModel(ILogger<IndexModel> logger, IProductService productService)
        {
            _logger = logger;
            
            _productService = productService;
        }

        public void OnGet()
        {
            try
            {
                Products = _productService.GetProducts();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Index Page {ex.ToString()}");
            }
           
        }
    }
}
