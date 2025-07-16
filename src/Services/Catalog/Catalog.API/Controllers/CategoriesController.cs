using Catalog.Application.Features.Categories.Commands.CreateCategory;
using Catalog.Application.Features.Categories.Commands.UpdateCategory;
using Catalog.Application.Features.Categories.Commands.DeleteCategory;
using Catalog.Application.Features.Categories.Queries.GetCategories;
using Catalog.Application.Features.Categories.Queries.GetCategoryById;
using Microsoft.AspNetCore.Mvc;


namespace Catalog.API.Controllers
{
    public class CategoriesController : ApiControllerBase
    {
        /// <summary>
        /// Get all categories
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<CategoryDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<CategoryDto>>> GetCategories([FromQuery] bool activeOnly = true)
        {
            return await Mediator.Send(new GetCategoriesQuery { ActiveOnly = activeOnly });
        }

        /// <summary>
        /// Get a specific category by id
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CategoryDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryDetailDto>> GetCategory(int id)
        {
            var category = await Mediator.Send(new GetCategoryByIdQuery(id));
            return Ok(category);
        }

        /// <summary>
        /// Create a new category
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> CreateCategory(CreateCategoryCommand command)
        {
            var id = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetCategory), new { id }, id);
        }

        /// <summary>
        /// Update an existing category
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await Mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Delete a category
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await Mediator.Send(new DeleteCategoryCommand(id));
            return NoContent();
        }
    }
}
