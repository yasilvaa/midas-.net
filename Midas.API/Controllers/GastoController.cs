using Microsoft.AspNetCore.Mvc;
using Midas.Infrastructure.Persistence.Entities;
using Midas.Infrastructure.Persistence.Repositories;
using Midas.API.DTOs;

namespace Midas.Controllers
{
    [ApiController]
    [Route("api/gastos")]
    public class GastoController : ControllerBase
    {
        private readonly IGastoRepository _gastoRepository;

        public GastoController(IGastoRepository gastoRepository)
        {
            _gastoRepository = gastoRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Gasto>>> GetAll()
        {
            return await _gastoRepository.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Gasto>> GetById(int id)
        {
            var gasto = await _gastoRepository.GetByIdAsync(id);
            if (gasto == null)
                return NotFound();

            return gasto;
        }

        [HttpPost]
        public async Task<ActionResult<Gasto>> Create([FromBody] CreateGastoDTO createGastoDTO)
        {
            if (createGastoDTO == null)
                return BadRequest("Dados do gasto são obrigatórios");

            var gasto = new Gasto
            {
                UsuarioId = createGastoDTO.UsuarioId,
                CategoriaId = createGastoDTO.CategoriaId,
                Titulo = createGastoDTO.Titulo,
                Data = createGastoDTO.Data,
                Valor = createGastoDTO.Valor,
                Fixo = NormalizeFixoValue(createGastoDTO.Fixo)
            };

            var result = await _gastoRepository.AddAsync(gasto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Gasto gasto)
        {
            if (id != gasto.Id)
                return BadRequest();

            gasto.Fixo = NormalizeFixoValue(gasto.Fixo);

            await _gastoRepository.UpdateAsync(gasto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _gastoRepository.DeleteAsync(id);
            return NoContent();
        }

        private char NormalizeFixoValue(char fixo)
        {
            return char.ToUpper(fixo) == 'T' ? 'T' : 'F';
        }
    }
}
