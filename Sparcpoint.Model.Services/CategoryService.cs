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
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger logger;


        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryService"/> class.
        /// </summary>
        /// <param name="categoryRepository">The category repository instance.</param>
        /// <param name="logger">The logger</param>
        /// <exception cref="ArgumentNullException">
        /// logger
        /// </exception>
        public CategoryService(ICategoryRepository categoryRepository, ILogger<CategoryService> logger)
        {
            this._categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <returns></returns>
        public async Task<List<Category>> GetAll()
        {
            var categories = await this._categoryRepository.GetAll().ConfigureAwait(false);

            return categories?.Select(p => new Category
            {
                Name = p.Name,
                Description = p.Description
            }).ToList();
        }

        /// <summary>
        /// Gets category details by category instance id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Category> GetById(int id)
        {
            var category = await this._categoryRepository.GetById(id).ConfigureAwait(false);

            if(category == null)
            {
                throw new CategoryNotFoundException("Category not found!");
            }

            return new Category
            {
                Name = category.Name,
                Description = category.Description
            };
        }

        /// <summary>
        /// Creates a new category
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<Category> Add(Category category)
        {
            var validations = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(category.Name))
            {
                validations.Add(new ValidationResult("Invalid category name"));
            }

            if (validations.Any())
            {
                throw new RqValidationFailedException(validations);
            }

            var categoryEntity = new CategoryEntity
            {
                Name = category.Name,
                Description = category.Description
            };

            await this._categoryRepository.Add(categoryEntity).ConfigureAwait(false);

            return category;
        }

        /// <summary>
        /// Updates existing category
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task Update(Category category)
        {
            var validations = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(category.Name))
            {
                validations.Add(new ValidationResult("Invalid category name"));
            }

            if (validations.Any())
            {
                throw new RqValidationFailedException(validations);
            }

            var categoryEntity = new CategoryEntity
            {
                Name = category.Name,
                Description = category.Description,
            };


            await this._categoryRepository.Update(categoryEntity).ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes category by category instance id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task Delete(int id)
        {
            await this._categoryRepository.Delete(id).ConfigureAwait(false);
        }
    }
}
