using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sparcpoint.Model.Services.Interfaces
{
    public interface IProductService
    {
        /// <summary>
        /// Gets all products
        /// </summary>
        /// <returns></returns>
        Task<List<Product>> GetAll();

        /// <summary>
        /// Gets product details by product instance id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Product> GetById(int id);

        /// <summary>
        /// Creates a new product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task<Product> Add(Product product);

        /// <summary>
        /// Updates existing product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task Update(Product product);

        /// <summary>
        /// Deletes product by product instance id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(int id);
    }
}
