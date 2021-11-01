using Microsoft.Extensions.Logging;
using Sparcpoint.Data;
using Sparcpoint.Data.Access.Interfaces;
using Sparcpoint.Infrastructure;
using Sparcpoint.Infrastructure.Exceptions;
using Sparcpoint.Model.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sparcpoint.Model.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger logger;


        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService"/> class.
        /// </summary>
        /// <param name="productRepository">The product repository instance.</param>
        /// <param name="logger">The logger</param>
        /// <exception cref="ArgumentNullException">
        /// logger
        /// </exception>
        public ProductService(IProductRepository productRepository, ILogger<ProductService> logger)
        {
            this._productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets all prodcuts
        /// </summary>
        /// <returns></returns>
        public async Task<List<Product>> GetAll()
        {
            var products = await this._productRepository.GetAll().ConfigureAwait(false);

            return products?.Select(p => new Product
            {
                Name = p.Name,
                Description = p.Description,
                ValidSkus = p.ValidSkus,
                ImageUris = p.ProductImageUris
            }).ToList();
        }

        /// <summary>
        /// Gets product details by product instance id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Product> GetById(int id)
        {
            var product = await this._productRepository.GetById(id).ConfigureAwait(false);

            return new Product
            {
                Name = product.Name,
                Description = product.Description,
                ValidSkus = product.ValidSkus,
                ImageUris = product.ProductImageUris
            };
        }

        /// <summary>
        /// Creates a new product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<Product> Add(Product product)
        {
            var validations = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(product.Name))
            {
                validations.Add(new ValidationResult("Invalid product name"));
            }

            if (product.CategoryId <= 0)
            {
                validations.Add(new ValidationResult("Invalid category."));
            }

            if (validations.Any())
            {
                throw new RqValidationFailedException(validations);
            }

            var productEntity = new ProductEntity
            {
                Name = product.Name,
                Description = product.Description,
                ProductImageUris = product.ImageUris,
                ValidSkus = product.ValidSkus
            };


            await this._productRepository.Add(productEntity, product.CategoryId).ConfigureAwait(false);

            return product;
        }

        /// <summary>
        /// Updates existing product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task Update(Product product)
        {
            var validations = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(product.Name))
            {
                validations.Add(new ValidationResult("Invalid product name"));
            }

            if (product.CategoryId <= 0)
            {
                validations.Add(new ValidationResult("Invalid category."));
            }

            var productEntity = new ProductEntity
            {
                Name = product.Name,
                Description = product.Description,
                ProductImageUris = product.ImageUris,
                ValidSkus = product.ValidSkus
            };


            await this._productRepository.Update(productEntity, product.CategoryId).ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes product by product instance id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task Delete(int id)
        {
            await this._productRepository.Delete(id).ConfigureAwait(false);
        }
    }
}
