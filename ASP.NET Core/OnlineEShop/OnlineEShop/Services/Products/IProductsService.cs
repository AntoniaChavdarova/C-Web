using OnlineEShop.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineEShop.Services.Products
{
    public interface IProductsService
    {
        Task CreateAsync(CreateProductInputModel input);

         IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();

         IEnumerable<CreateProductInputModel>  AllProductsInCategory(int id);

        CreateProductInputModel ById(int id);
    }
}
