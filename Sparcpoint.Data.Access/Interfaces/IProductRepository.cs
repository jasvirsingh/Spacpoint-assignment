using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sparcpoint.Data.Access.Interfaces
{
    public interface IProductRepository
    {
        Task<List<ProductEntity>> GetAll();

        Task<ProductEntity> GetById(int id);
 
        Task<ProductEntity> Add(ProductEntity productEntity, int categoryId);
        
        Task Update(ProductEntity productEntity, int categoryId);
        
        Task Delete(int id);
    }
}
