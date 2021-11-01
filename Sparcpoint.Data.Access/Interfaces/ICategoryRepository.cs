using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sparcpoint.Data.Access.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<CategoryEntity>> GetAll();

        Task<CategoryEntity> GetById(int id);

        Task<CategoryEntity> Add(CategoryEntity productEntity);

        Task Update(CategoryEntity productEntity);

        Task Delete(int id);
    }
}
