using Catalog.Application.Common.Models;
using Catalog.Application.Features.Products.Commands.CreateProduct;
using Catalog.Application.Features.Products.Commands.DeleteProduct;
using Catalog.Application.Features.Products.Commands.UpdateProduct;
using Catalog.Application.Features.Products.Commands.UpdateStock;
using Catalog.Application.Features.Products.Queries.GetProductById;
using Catalog.Application.Features.Products.Queries.GetProducts;
using Microsoft.AspNetCore.Mvc;


namespace Catalog.API.Controllers
{
    public class ProductsController : ApiControllerBase
    {
        /// <summary>
        /// Get products with pagination and filtering
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<ProductDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<ProductDto>>> GetProducts([FromQuery] GetProductsQuery query)
        {
            return await Mediator.Send(query);
        }

        /// <summary>
        /// Get a specific product by id
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDetailDto>> GetProduct(int id)
        {
            var product = await Mediator.Send(new GetProductByIdQuery(id));
            return Ok(product);
        }

        /// <summary>
        /// Create a new product
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> CreateProduct(CreateProductCommand command)
        {
            var id = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetProduct), new { id }, id);
        }

        /// <summary>
        /// Update an existing product
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProduct(int id, UpdateProductCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await Mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Delete a product
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await Mediator.Send(new DeleteProductCommand(id));
            return NoContent();
        }

        /// <summary>
        /// Update product stock
        /// </summary>
        [HttpPatch("{id}/stock")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStock(int id, UpdateStockCommand command)
        {
            if (id != command.ProductId)
            {
                return BadRequest();
            }

            await Mediator.Send(command);
            return NoContent();
        }
    }
}
