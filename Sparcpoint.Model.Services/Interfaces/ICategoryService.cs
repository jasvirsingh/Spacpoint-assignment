using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sparcpoint.Model.Services.Interfaces
{
    public interface ICategoryService
    {
        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <returns></returns>
        Task<List<Category>> GetAll();

        /// <summary>
        /// Gets category details by category instance id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Category> GetById(int id);

        /// <summary>
        /// Creates a new category
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task<Category> Add(Category product);

        /// <summary>
        /// Updates existing category
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task Update(Category product);

        /// <summary>
        /// Deletes category by category instance id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(int id);
    }
}
