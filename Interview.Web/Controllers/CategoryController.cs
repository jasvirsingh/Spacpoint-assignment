using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sparcpoint.Model;
using Sparcpoint.Model.Services.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Interview.Web.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/v1/categories")]

    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductController"/> class.
        /// </summary>
        /// <param name="categoryService">The category service instance.</param>
        /// <param name="logger">The logger</param>

        public CategoryController(ICategoryService categoryService, ILogger logger)
        {
            this._categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [Route("categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var result = await this._categoryService.GetAll().ConfigureAwait(false);

            return Ok(result);
        }

        [HttpPost]
        [Route("/category")]
        public async Task<IActionResult> CreateProduct([FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest();
            }

            try
            {
                var result = await this._categoryService.Add(category).ConfigureAwait(false);

                return Ok(result);
            }
            catch (Exception ex)
            {
                this.logger?.LogError(ex, $"Error occured while creating category. {ex.Message}");

                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("/category")]
        public async Task<ActionResult> Update([FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest();
            }

            try
            {
                await this._categoryService.Update(category).ConfigureAwait(false);

                return Ok(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                this.logger?.LogError(ex, $"Error occured while updating category. {ex.Message}");

                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("/categories/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await this._categoryService.Delete(id).ConfigureAwait(false);

                return Ok(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                this.logger?.LogError(ex, $"Error occured while deleting category. {ex.Message}");

                return BadRequest(ex.Message);
            }
        }
    }
}
