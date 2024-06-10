using EasyPayChallenge.Domain;
using EasyPayChallenge.Domain.Models.Commands;
using EasyPayChallenge.Domain.Models.Dto;
using EasyPayChallenge.Domain.Models.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProductController> _logger;

    public ProductController(IMediator mediator, ILogger<ProductController> logger)
    {
        _mediator = mediator;   
        _logger = logger;
    }

    [HttpGet, Route("{id:min(1)}")]
    public async Task<ActionResult<ProductDto>> GetProduct([FromRoute] int id, CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetProductByIdQuery { Id = id };
            var result = await _mediator.Send(query, cancellationToken);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    [HttpGet, Route("all")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts([FromQuery] GetAllProductsQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
    {
        try
        {
            // Check if the provided string is a valid enum value
            if (!Enum.IsDefined(typeof(Currency), command.Currency))
            {
                // If the string is not a valid enum value, throw an exception or handle the error accordingly
                return StatusCode(StatusCodes.Status400BadRequest,$"Invalid currency: {command.Currency}");
            }
            
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    [HttpPatch]
    public async Task<ActionResult<ProductDto>> UpdateProduct([FromBody] UpdateProductCommand command, CancellationToken cancellationToken)
    {
        try
        {
            // Check if the provided string is a valid enum value
            if (!Enum.IsDefined(typeof(Currency), command.Currency))
            {
                // If the string is not a valid enum value, throw an exception or handle the error accordingly
                return StatusCode(StatusCodes.Status400BadRequest,$"Invalid currency: {command.Currency}");
            }
            
            var result = await _mediator.Send(command, cancellationToken);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }
    
    [HttpDelete,  Route("{id:min(1)}")]
    public async Task<ActionResult<ProductDto>> DeleteProduct([FromRoute] int id, CancellationToken cancellationToken)
    {
        try
        {
            var command = new DeleteProductCommand
            {
                Id = id
            };
            
            var result = await _mediator.Send(command, cancellationToken);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }
}
