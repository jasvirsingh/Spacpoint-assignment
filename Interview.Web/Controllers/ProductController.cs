using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sparcpoint.Infrastructure;
using Sparcpoint.Infrastructure.Exceptions;
using Sparcpoint.Model;
using Sparcpoint.Model.Services.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Interview.Web.Controllers
{
    [Route("api/v1/products")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductController"/> class.
        /// </summary>
        /// <param name="productService">The product service instance.</param>
        /// <param name="logger">The logger</param>

        public ProductController(IProductService productService, ILogger logger)
        {
            this._productService = productService ?? throw new ArgumentNullException(nameof(productService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [Route("products")]
        public async Task<IActionResult> Get()
        {
            var result = await this._productService.GetAll().ConfigureAwait(false);

            return Ok(result);
        }

        [HttpGet]
        [Route("products/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await this._productService.GetById(id).ConfigureAwait(false);

            return Ok(result);
        }

        [HttpGet]
        [Route("products/count")]
        public async Task<IActionResult> GetproductsCount()
        {
            var result = await this._productService.GetAll().ConfigureAwait(false);

            return Ok(result.Count);
        }

        [HttpGet]
        [Route("products/{categoryName}")]
        public async Task<IActionResult> GetProductsByCategory(string categoryName)
        {
            var result = await this._productService.GetAll().ConfigureAwait(false);

            return Ok(result);
        }

        [HttpPost]
        [Route("/product")]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            if (product == null)
            {
                this.logger?.LogError("Invalid create product request. Product can not be null.");

                return BadRequest();
            }

            try
            {
                var result = await this._productService.Add(product).ConfigureAwait(false);

                return Ok(result);
            }
            catch (RqValidationFailedException ex)
            {
                return BadRequest(ex.Messages);
            }
            catch (CategoryNotFoundException ex)
            {
                this.logger?.LogError(ex, $"Category id {product.CategoryId} does not exist!");

                return BadRequest(new ValidationResult(ex.Message));
            }
            catch (DuplicateProductException ex)
            {
                this.logger?.LogError(ex, $"Product {product.Name} already exists");

                return BadRequest(new ValidationResult(ex.Message));
            }
            catch (Exception ex)
            {
                this.logger?.LogError(ex, $"Error occured while creating product. {ex.Message}");

                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("/product")]
        public async Task<ActionResult> Update([FromBody] Product product)
        {
            if (product == null)
            {
                this.logger?.LogError("Invalid product request. Product can not be null.");

                return BadRequest();
            }

            try
            {
                await this._productService.Update(product).ConfigureAwait(false);

                return Ok(HttpStatusCode.NoContent);
            }
            catch (ProductNotFoundException ex)
            {
                this.logger?.LogError(ex, $"Product {product.Name} not found!");

                return BadRequest(new ValidationResult(ex.Message));
            }
            catch (CategoryNotFoundException ex)
            {
                this.logger?.LogError(ex, $"Category id {product.CategoryId} does not exist!");

                return BadRequest(new ValidationResult(ex.Message));
            }
            catch (Exception ex)
            {
                this.logger?.LogError(ex, $"Error occured while updating product. {ex.Message}");

                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("/products/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await this._productService.Delete(id).ConfigureAwait(false);

                return Ok(HttpStatusCode.NoContent);
            }
            catch (ProductNotFoundException ex)
            {
                this.logger?.LogError(ex, $"Product instance id {id} not found!");

                return BadRequest(new ValidationResult(ex.Message));
            }
            catch (Exception ex)
            {
                this.logger?.LogError(ex, $"Error occured while deleting product. {ex.Message}");

                return BadRequest(ex.Message);
            }
        }
    }
}
