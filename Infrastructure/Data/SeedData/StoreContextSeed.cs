using Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Infrastructure.Data.SeedData
{
    public class StoreContextSeed
    {
        private const string SEED_DATA_PATH = "C:/store/Infrastructure/Data/SeedData/";
        public static async Task SeedAsync(StoreDataContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                

                if (!context.ProductBrands.Any())
                {

                    var brandsData = File.ReadAllText(SEED_DATA_PATH + "brands.json");
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                    foreach (var brand in brands) 
                    { 
                        context.ProductBrands.Add(brand);
                    }

                    await context.SaveChangesAsync();

                }

                if (!context.ProductTypes.Any())
                {

                    var typesData = File.ReadAllText(SEED_DATA_PATH + "types.json");
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                    foreach (var type in types)
                    {
                        context.ProductTypes.Add(type);
                    }

                    await context.SaveChangesAsync();

                }

                if (!context.Products.Any())
                {

                    var productsData = File.ReadAllText(SEED_DATA_PATH + "products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                    foreach (var product in products)
                    {
                        context.Products.Add(product);
                    }

                    await context.SaveChangesAsync();

                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}
