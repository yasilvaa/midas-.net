using Microsoft.AspNetCore.Mvc;
using Midas.Infrastructure.Persistence.Entities;
using Midas.Infrastructure.Persistence.Repositories;

namespace Midas.Controllers
{
    [ApiController]
    [Route("api/cofrinhos")]
    public class CofrinhoController : ControllerBase
    {
        private readonly ICofrinhoRepository _cofrinhoRepository;

        public CofrinhoController(ICofrinhoRepository cofrinhoRepository)
        {
            _cofrinhoRepository = cofrinhoRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Cofrinho>>> GetAll()
        {
            return await _cofrinhoRepository.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cofrinho>> GetById(int id)
        {
            var cofrinho = await _cofrinhoRepository.GetByIdAsync(id);
            if (cofrinho == null)
                return NotFound();

            return cofrinho;
        }

        [HttpPost]
        public async Task<ActionResult<Cofrinho>> Create([FromBody] Cofrinho cofrinho)
        {
            var result = await _cofrinhoRepository.AddAsync(cofrinho);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Cofrinho cofrinho)
        {
            if (id != cofrinho.Id)
                return BadRequest();

            await _cofrinhoRepository.UpdateAsync(cofrinho);
            return NoContent();
        }

        [HttpPut("{id}/progresso")]
        public async Task<IActionResult> AtualizarProgresso(int id, [FromBody] decimal valorAtingido)
        {
            var result = await _cofrinhoRepository.AtualizarProgresso(id, valorAtingido);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _cofrinhoRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
