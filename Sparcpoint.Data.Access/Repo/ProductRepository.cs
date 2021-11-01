using Microsoft.EntityFrameworkCore;
using Sparcpoint.Data.Access.EF;
using Sparcpoint.Data.Access.Interfaces;
using Sparcpoint.Infrastructure.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sparcpoint.Data.Access.Repo
{
    public class ProductRepository : IProductRepository
    {
        private readonly SparcpointDbContext _context;
        public ProductRepository()
        {
            _context = new SparcpointDbContext();
        }

        public async Task<List<ProductEntity>> GetAll()
        {
            return await _context.Products.ToListAsync().ConfigureAwait(false);
        }

        public async Task<ProductEntity> GetById(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.InstanceId == id);
        }

        public async Task<ProductEntity> Add(ProductEntity productEntity, int categoryId)
        {
            bool isCategoryExists = await _context.Categories.AnyAsync(c => c.InstanceId == categoryId);
            if (!isCategoryExists)
            {
                throw new CategoryNotFoundException("Product category not found!");
            }

            bool isProductExists = await _context.Products.AnyAsync(p => p.Name == productEntity.Name);
            if (isProductExists)
            {
                throw new DuplicateProductException("Duplicate product!");
            }

            // create product
            _context.Products.Add(productEntity);

            await _context.SaveChangesAsync().ConfigureAwait(false);

            // add product category details

            var newProduct = await _context.Products.FirstOrDefaultAsync(p => p.Name == productEntity.Name).ConfigureAwait(false);

            var productCategoryEntity = new ProductCategoryEntity
            {
                CategoryInstanceId = categoryId,
                InstanceId = newProduct.InstanceId
            };

            await _context.SaveChangesAsync().ConfigureAwait(false);

            return newProduct;
        }

        public async Task Update(ProductEntity productEntity, int categoryId)
        {
            bool isCategoryExists = await _context.Categories.AnyAsync(c => c.InstanceId == categoryId);
            if (!isCategoryExists)
            {
                throw new CategoryNotFoundException("Product category not found!");
            }

            bool exists = await _context.Products.AnyAsync(p => p.InstanceId == productEntity.InstanceId).ConfigureAwait(false);
            if (!exists)
            {
                throw new ProductNotFoundException("Product not found!");
            }

            var product = await _context.Products.FirstAsync(p => p.InstanceId == productEntity.InstanceId);

            product.Name = productEntity.Name;
            product.Description = productEntity.Description;
            product.ProductImageUris = productEntity.ProductImageUris;
            product.ValidSkus = productEntity.ValidSkus;

            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Delete(int id)
        {
            bool exists = await _context.Products.AnyAsync(c => c.InstanceId == id).ConfigureAwait(false);
            if (!exists)
            {
                throw new ProductNotFoundException("Product not found!");
            }

            var product = await _context.Products.FirstAsync(c => c.InstanceId == id).ConfigureAwait(false);
            
            _context.Products.Remove(product);

            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
