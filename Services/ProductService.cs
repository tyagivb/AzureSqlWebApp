using AzureSqlWebApp.Models;
using Microsoft.FeatureManagement;
using System.Data.SqlClient;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AzureSqlWebApp.Services
{
    public class ProductService : IProductService
    {
        private readonly IConfiguration _configuration;
        private readonly IFeatureManager _featureManager;
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IConfiguration configuration, IFeatureManager featureManager, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _featureManager = featureManager;
            _httpClientFactory = httpClientFactory;
        }
        private SqlConnection GetConnection()
        {

            return new SqlConnection(_configuration["SQLConnection"]);
        }

        public async Task<bool> IsBetaAsync() {
            var isBeta = await _featureManager.IsEnabledAsync("beta");
            return isBeta;
        }
        public List<Product> GetProducts()
        {
            List<Product> _product_lst = new List<Product>();
            string _statement = "SELECT ProductID,ProductName,Quantity from Products";
            SqlConnection _connection = GetConnection();

            _connection.Open();

            SqlCommand _sqlcommand = new SqlCommand(_statement, _connection);

            using (SqlDataReader _reader = _sqlcommand.ExecuteReader())
            {
                while (_reader.Read())
                {
                    Product _product = new Product()
                    {
                        ProductID = _reader.GetInt32(0),
                        ProductName = _reader.GetString(1),
                        Quantity = _reader.GetInt32(2)
                    };

                    _product_lst.Add(_product);
                }
            }
            _connection.Close();
            return _product_lst;
        }

        public async Task<List<Product>> GetProductsFromFunctionAppAsync()
        {
            string funtionUrl = "https://azfunctionapp003.azurewebsites.net/api/GetProducts?code=MS1FLGtjoKJaYVkglHz44U1GTf54fBj1x-gcq7RwB2dHAzFuEgu_TQ==";
            using (var client = _httpClientFactory.CreateClient())
            {
                HttpResponseMessage data = await client.GetAsync(funtionUrl);

                var content = await data.Content.ReadAsStringAsync();

                var productList = JsonSerializer.Deserialize<List<Product>>(content);

                return productList;

            }
        }

    }
}
