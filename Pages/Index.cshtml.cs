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
        public bool IsBeta { get; set; }
        public IndexModel(ILogger<IndexModel> logger, IProductService productService)
        {
            _logger = logger;
            
            _productService = productService;
        }

        public async Task OnGet()
        {
            try
            {
                IsBeta = await _productService.IsBetaAsync();
                Products = await _productService.GetProductsFromFunctionAppAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Index Page {ex.ToString()}");
            }
           
        }
    }
}
