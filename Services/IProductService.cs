using AzureSqlWebApp.Models;

namespace AzureSqlWebApp.Services
{
    public interface IProductService
    {
        List<Product> GetProducts();
        Task<bool> IsBetaAsync();
        Task<List<Product>> GetProductsFromFunctionAppAsync();
    }
}