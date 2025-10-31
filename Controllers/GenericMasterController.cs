using GenericController.CQRS.Commands;
using GenericController.CQRS.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace GenericController.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenericMasterController<T, TDto> : ControllerBase where T : class where TDto : class
    {
        protected readonly IMediator _mediator;
        protected readonly ILogger<GenericMasterController<T, TDto>> _logger;

        public GenericMasterController(IMediator mediator, ILogger<GenericMasterController<T, TDto>> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var query = new GetAllQuery<TDto>();
                var items = await _mediator.Send(query);
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all entities");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(object id)
        {
            try
            {
                var query = new GetByIdQuery<TDto>(id);
                var item = await _mediator.Send(query);
                if (item == null)
                {
                    return NotFound();
                }
                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving entity with id {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var command = new CreateCommand<T, TDto>(dto);
                var createdItem = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetById), new { id = GetIdFromDto(createdItem) }, createdItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating entity");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] object id, [FromBody] TDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var command = new UpdateCommand<T, TDto>(id, dto);
                await _mediator.Send(command);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating entity with id {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(object id)
        {
            try
            {
                var command = new DeleteCommand<T>(id);
                await _mediator.Send(command);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting entity with id {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}/soft")]
        public async Task<IActionResult> SoftDelete(object id)
        {
            try
            {
                var command = new SoftDeleteCommand<T>(id);
                await _mediator.Send(command);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error soft deleting entity with id {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? filter = null)
        {
            try
            {
                var query = new GetPagedQuery<TDto>(pageNumber, pageSize, filter);
                var result = await _mediator.Send(query);
                return Ok(new
                {
                    Items = result.Items,
                    TotalCount = result.TotalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling(result.TotalCount / (double)pageSize)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving paged entities");
                return StatusCode(500, "Internal server error");
            }
        }

        protected virtual object GetIdFromDto(TDto dto)
        {
            // This method should be overridden in derived controllers to extract the ID from the DTO
            // For now, return a default value
            return dto.GetHashCode();
        }

        protected virtual Expression<Func<T, bool>>? CreateFilterExpression(string filter)
        {
            // This method should be overridden in derived controllers to create filter expressions
            // For now, return null
            return null;
        }
    }
}