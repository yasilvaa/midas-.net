using Microsoft.AspNetCore.Mvc;
using Midas.Infrastructure.Persistence.Entities;
using Midas.Infrastructure.Persistence.Repositories;
using Midas.API.DTOs;

namespace Midas.Controllers
{
    [ApiController]
    [Route("api/categorias")]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaController(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Categoria>>> GetAll()
        {
            return await _categoriaRepository.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> GetById(int id)
        {
            var categoria = await _categoriaRepository.GetByIdAsync(id);
            if (categoria == null)
                return NotFound();

            return categoria;
        }

        [HttpPost]
        public async Task<ActionResult<Categoria>> Create([FromBody] CreateCategoriaDTO createCategoriaDTO)
        {
            if (createCategoriaDTO == null)
                return BadRequest("Dados da categoria são obrigatórios");

            var categoria = new Categoria
            {
                Nome = createCategoriaDTO.Nome,
                Descricao = createCategoriaDTO.Descricao
                // gera ID automático
            };

            var result = await _categoriaRepository.AddAsync(categoria);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Categoria categoria)
        {
            if (id != categoria.Id)
                return BadRequest();

            await _categoriaRepository.UpdateAsync(categoria);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoriaRepository.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<PagedResult<Categoria>>> Search([FromQuery] CategoriaSearchParameters parameters)
        {
            try
            {
                var result = await _categoriaRepository.SearchAsync(parameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno do servidor");
            }
        }
    }
}