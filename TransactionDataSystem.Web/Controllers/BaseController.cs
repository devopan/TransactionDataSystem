using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TransactionDataSystem.Services.DTOs;

namespace TransactionDataSystem.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController<TDto, TCreateDto, TIdType> : ControllerBase
        where TDto : class
        where TCreateDto : class
    {
        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(TIdType id)
        {
            var entity = await ReadSingleAsync(id);
            if (entity == null)
                return NotFound();

            return Ok(entity);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Create(TCreateDto createDto)
        {
            var createdEntity = await CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = GetEntityId(createdEntity) }, createdEntity);
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<TDto>>> GetAll()
        {
            var users = await GetAllAsync();
            return Ok(users);
        }

        protected abstract Task<TDto> ReadSingleAsync(TIdType id);
        protected abstract Task<TDto> CreateAsync(TCreateDto createDto);
        protected abstract TIdType GetEntityId(TDto entity);
        protected abstract Task<IEnumerable<TDto>> GetAllAsync();
    }
}