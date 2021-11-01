using Microsoft.EntityFrameworkCore;
using Sparcpoint.Data.Access.EF;
using Sparcpoint.Data.Access.Interfaces;
using Sparcpoint.Infrastructure.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sparcpoint.Data.Access.Repo
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly SparcpointDbContext _context;
        public CategoryRepository()
        {
            _context = new SparcpointDbContext();
        }

        public async Task<List<CategoryEntity>> GetAll()
        {
            return await _context.Categories.ToListAsync().ConfigureAwait(false);
        }

        public async Task<CategoryEntity> GetById(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.InstanceId == id);
        }

        public async Task<CategoryEntity> Add(CategoryEntity categoryEntity)
        {
            bool exists = await _context.Categories.AnyAsync(p => p.Name == categoryEntity.Name);
            if (exists)
            {
                throw new DuplicateCategoryException("Duplicate category!");
            }

            _context.Categories.Add(categoryEntity);

            await _context.SaveChangesAsync().ConfigureAwait(false);

            return await _context.Categories.FirstOrDefaultAsync(p => p.Name == categoryEntity.Name);
        }

        public async Task Update(CategoryEntity categoryEntity)
        {
            bool exists = await _context.Categories.AnyAsync(c => c.InstanceId == categoryEntity.InstanceId).ConfigureAwait(false);
            if (!exists)
            {
                throw new CategoryNotFoundException("Category not found!");
            }

            var category = await _context.Categories.FirstAsync(p => p.InstanceId == categoryEntity.InstanceId);

            category.Description = categoryEntity.Description;
           
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Delete(int id)
        {
            bool exists = await _context.Categories.AnyAsync(c => c.InstanceId == id).ConfigureAwait(false);
            if (!exists)
            {
                throw new CategoryNotFoundException("Category not found!");
            }

            var category = await _context.Categories.FirstAsync(c => c.InstanceId == id).ConfigureAwait(false);

            _context.Categories.Remove(category);

            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}